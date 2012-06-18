using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PushSharp.ClientSample.WindowsPhone
{
	public partial class MainPage : PhoneApplicationPage
	{
		PushSharpClient client;

		// Constructor
		public MainPage()
		{
			InitializeComponent();

			client = new PushSharpClient();
			client.RegisterForToast();
		}
	}
}