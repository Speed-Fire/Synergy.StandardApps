#include "pch.h"
#include "PerMonitorDPIHelper.h"

/// <summary>
/// Sets the current process as Per_Monitor_DPI_Aware. Returns True if the process was marked as Per_Monitor_DPI_Aware
/// </summary>
BOOL PerMonitorDPILibrary::PerMonitorDPIHelper::SetPerMonitorDPIAware()
{
	auto result = SetProcessDpiAwareness(PROCESS_DPI_AWARENESS::PROCESS_PER_MONITOR_DPI_AWARE);

	if (S_OK != result)
	{
		return FALSE;
	}
	return TRUE;
}

/// <summary>
/// Returns the DPI awareness of the current process
/// </summary>
PROCESS_DPI_AWARENESS PerMonitorDPILibrary::PerMonitorDPIHelper::GetPerMonitorDPIAware()
{
	PROCESS_DPI_AWARENESS awareness;
	HANDLE hProcess;
	hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, GetCurrentProcessId());
	auto result = GetProcessDpiAwareness(hProcess, &awareness);

	if (S_OK != result)
	{
		throw gcnew System::Exception(L"Unable to read process DPI level");
	}
	return awareness;
}

/// <summary>
/// Returns the DPI of the window handle passed in the parameter 
/// </summary>
double PerMonitorDPILibrary::PerMonitorDPIHelper::GetDpiForWindow(IntPtr hwnd)
{
	return GetDpiForHwnd(static_cast<HWND>(hwnd.ToPointer()));
}

double PerMonitorDPILibrary::PerMonitorDPIHelper::GetDpiForHwnd(HWND hWnd)
{
	auto monitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
	UINT newDpiX;
	UINT newDpiY;
	if (FAILED(GetDpiForMonitor(monitor, MONITOR_DPI_TYPE::MDT_EFFECTIVE_DPI, &newDpiX, &newDpiY)))
	{
		newDpiX = 96;
		newDpiY = 96;
	}
	return ((double)newDpiX);
}

/// <summary>
/// //Returns the system DPI
/// </summary>
double PerMonitorDPILibrary::PerMonitorDPIHelper::GetSystemDPI()
{
	int newDpiX(0);
	auto hDC = GetDC(NULL);
	newDpiX = GetDeviceCaps(hDC, LOGPIXELSX);
	ReleaseDC(NULL, hDC);
	return (double)newDpiX;
}