#pragma once
#include "pch.h"

using namespace System;

namespace PerMonitorDPILibrary {
	public ref class PerMonitorDPIHelper
	{
	public:

		static BOOL SetPerMonitorDPIAware();

		static PROCESS_DPI_AWARENESS GetPerMonitorDPIAware();

		static double GetDpiForWindow(IntPtr hwnd);

		static double GetSystemDPI();

	private:
		static double GetDpiForHwnd(HWND hWnd);
	};
}
