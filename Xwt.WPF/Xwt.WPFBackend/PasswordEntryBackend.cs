﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Xwt.WPFBackend;
using Xwt.Backends;
using System.Windows.Documents;
using System.Windows.Media;

namespace Xwt.WPFBackend
{
	class PasswordEntryBackend : WidgetBackend, IPasswordEntryBackend
	{
		PlaceholderTextAdorner Adorner {
			get; set;
		}
		public PasswordEntryBackend ()
		{
			Widget = new PasswordBox ();
			Adorner = new PlaceholderTextAdorner (PasswordBox);
			PasswordBox.Loaded += delegate {
				AdornerLayer.GetAdornerLayer (PasswordBox).Add (Adorner);
			};
		}

		protected PasswordBox PasswordBox
		{
			get { return (PasswordBox) Widget; }
		}

		public string Password
		{
			get { return PasswordBox.Password; }
			set { PasswordBox.Password = value; }
		}

		public System.Security.SecureString SecurePassword
		{
			get { return PasswordBox.SecurePassword; }
		}

		public string PlaceholderText {
			get {
				return Adorner.PlaceholderText;
			}
			set {
				Adorner.PlaceholderText = value;
			}
		}

		public override void EnableEvent (object eventId)
		{
			base.EnableEvent (eventId);

			if (eventId is PasswordEntryEvent) 
			{
				switch ((PasswordEntryEvent) eventId) 
				{
					case PasswordEntryEvent.Changed:
						PasswordBox.PasswordChanged += OnPasswordChanged;
						break;
				}
			}
		}

		public override void DisableEvent (object eventId)
		{
			base.DisableEvent (eventId);

			if (eventId is PasswordEntryEvent)
			{
				switch ((PasswordEntryEvent)eventId)
				{
					case PasswordEntryEvent.Changed:
						PasswordBox.PasswordChanged -= OnPasswordChanged;
						break;
				}
			}
		}

		protected new IPasswordEntryEventSink EventSink {
			get { return (IPasswordEntryEventSink) base.EventSink; }
		}

		void OnPasswordChanged (object s, RoutedEventArgs e)
		{
			Context.InvokeUserCode (EventSink.OnChanged);
		}
	}
}
