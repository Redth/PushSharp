using System;
using System.Collections.Generic;
using System.Text;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace PushSharp.Web.Helpers
{
    class EncryptionHelper
    {
        public static EncryptionResult Encrypt(WebPushSubscription subscription, string payload)
        {

            if (string.IsNullOrEmpty(payload))
            {
                return null;
            }

            subscription.Validate();

            var keys = subscription.Keys;
            
            var data = Encoding.UTF8.GetBytes(payload);
            var userKey = WebEncoder.Base64UrlDecode(keys.P256dh);
            var userSecret = WebEncoder.Base64UrlDecode(keys.Auth);

            return EncryptMessage(userKey, userSecret, data);
        }

        //thanks to http://seehashrun.blogspot.com.by/2016/09/encrypting-data-for-web-browser-push.html
        public static EncryptionResult EncryptMessage(byte[] userKey, byte[] userSecret, byte[] data,
            ushort padding = 0, bool randomisePadding = false)
        {
            SecureRandom random = new SecureRandom();
            byte[] salt = new byte[16];
            random.NextBytes(salt);

            X9ECParameters curve = ECNamedCurveTable.GetByName("prime256v1");
            ECDomainParameters spec = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());
            ECKeyPairGenerator generator = new ECKeyPairGenerator();
            generator.Init(new ECKeyGenerationParameters(spec, new SecureRandom()));
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            ECDHBasicAgreement agreementGenerator = new ECDHBasicAgreement();
            agreementGenerator.Init(keyPair.Private);

            BigInteger ikm = agreementGenerator.CalculateAgreement(new ECPublicKeyParameters(spec.Curve.DecodePoint(userKey), spec));

            byte[] publicKey = ((ECPublicKeyParameters)keyPair.Public).Q.GetEncoded(false);

            byte[] prk = GenerateHKDF(userSecret, ikm.ToByteArrayUnsigned(), Encoding.UTF8.GetBytes("Content-Encoding: auth\0"), 32);
            byte[] cek = GenerateHKDF(salt, prk, CreateInfoChunk("aesgcm", userKey, publicKey), 16);
            byte[] nonce = GenerateHKDF(salt, prk, CreateInfoChunk("nonce", userKey, publicKey), 12);

            if (randomisePadding && padding > 0) padding = Convert.ToUInt16(Math.Abs(random.NextInt()) % (padding + 1));

            byte[] input = new byte[padding + 2 + data.Length];
            Buffer.BlockCopy(ConvertInt(padding), 0, input, 0, 2);
            Buffer.BlockCopy(data, 0, input, padding + 2, data.Length);

            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/GCM/NoPadding");
            cipher.Init(true, new AeadParameters(new KeyParameter(cek), 128, nonce));
            byte[] message = new byte[cipher.GetOutputSize(input.Length)];
            cipher.DoFinal(input, 0, input.Length, message, 0);

            return new EncryptionResult
            {
                Salt = salt,
                Payload = message,
                PublicKey = publicKey
            };
        }

        public static byte[] ConvertInt(int number)
        {
            byte[] output = BitConverter.GetBytes(Convert.ToUInt16(number));
            if (BitConverter.IsLittleEndian) Array.Reverse(output);
            return output;
        }

        public static byte[] CreateInfoChunk(string type, byte[] recipientPublicKey, byte[] senderPublicKey)
        {
            List<byte> output = new List<byte>();
            output.AddRange(Encoding.UTF8.GetBytes($"Content-Encoding: {type}\0P-256\0"));
            output.AddRange(ConvertInt(recipientPublicKey.Length));
            output.AddRange(recipientPublicKey);
            output.AddRange(ConvertInt(senderPublicKey.Length));
            output.AddRange(senderPublicKey);
            return output.ToArray();
        }

        public static byte[] GenerateHKDF(byte[] salt, byte[] ikm, byte[] info, int len)
        {
            IMac prkGen = MacUtilities.GetMac("HmacSHA256");
            prkGen.Init(new KeyParameter(MacUtilities.CalculateMac("HmacSHA256", new KeyParameter(salt), ikm)));
            prkGen.BlockUpdate(info, 0, info.Length);
            prkGen.Update((byte)1);
            byte[] result = MacUtilities.DoFinal(prkGen);
            if (result.Length > len) Array.Resize(ref result, len);
            return result;
        }
    }
}