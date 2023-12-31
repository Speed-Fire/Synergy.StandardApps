#pragma once

#include "PerMonitorDPIHelper.h"

using namespace System;
using namespace System::Windows::Interop;
using namespace System::Windows;
using namespace System::Windows::Media;
using namespace System::Security::Permissions;

namespace PerMonitorDPILibrary {

	public ref class PerMonitorDPIWindow : public Window
	{
	public:
		PerMonitorDPIWindow(void);

		event EventHandler^ DPIChanged;

		System::String^ GetCurrentDpiConfiguration();

		property double CurrentDPI
		{
			double get()
			{
				return m_currentDPI;
			}
		}

		property double WpfDPI
		{
			double get()
			{
				return m_wpfDPI;
			}
			void set(double value)
			{
				m_wpfDPI = value;
			}
		}

		property double ScaleFactor
		{
			double get()
			{
				return m_scaleFactor;
			}
		}

	protected:
		~PerMonitorDPIWindow();

		//[EnvironmentPermissionAttribute(SecurityAction::LinkDemand, Unrestricted = true)]
			void OnLoaded(Object^ sender, RoutedEventArgs^ args);

		void OnDPIChanged();

		void UpdateLayoutTransform(double scaleFactor);

		IntPtr HandleMessages(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, bool% handled);

	private:
		System::Boolean m_perMonitorEnabled;

		double m_currentDPI;

		double m_systemDPI;

		double m_wpfDPI;

		double m_scaleFactor;

		System::Windows::Interop::HwndSource^ m_source;
	};

}