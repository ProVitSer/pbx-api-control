using System;
using System.IO;
using System.Text;

public static class CustomLogger
{
    private static StreamWriter _logWriter = null;
    private static int _openCounter = 0;
    private static bool _dialerLogFlag = false;
    private static uint _writeCounter = 0;
    private static readonly object _lockObj = new object();

    private static void CloseLogFile()
    {
        if (_logWriter == null)
            return;

        _logWriter.Dispose();
        _logWriter = null;
    }

    private static void OpenLogFile()
    {
        try
        {
            _logWriter = new StreamWriter(PbxApiControl.PBXAPIConfig.LOG_PATH + PbxApiControl.PBXAPIConfig.LOG_NAME, true, Encoding.UTF8);
        }
        catch
        {
            CloseLogFile();
        }
    }

    private static void BackupLargeFile(long fileSizeLimit = -1)
    {
        if (fileSizeLimit < 0)
            fileSizeLimit = 1024 * 1024; // Default to 1 MB

        try
        {
            var logFilePath = PbxApiControl.PBXAPIConfig.LOG_PATH + PbxApiControl.PBXAPIConfig.LOG_NAME;
            var logFileInfo = new FileInfo(logFilePath);

            if (!logFileInfo.Exists || logFileInfo.Length <= fileSizeLimit)
                return;

            var backupFilePath = PbxApiControl.PBXAPIConfig.LOG_PATH + PbxApiControl.PBXAPIConfig.BACKUP_LOG_NAME;
            var backupFileInfo = new FileInfo(backupFilePath);

            if (backupFileInfo.Exists)
                backupFileInfo.Delete();

            logFileInfo.MoveTo(backupFilePath);
        }
        catch
        { }
    }

    public static void OpenLog()
    {
        lock (_lockObj)
        {
            if (!_dialerLogFlag && _openCounter++ == 0)
            {
                BackupLargeFile();
                OpenLogFile();
            }
            else if (_logWriter == null)
            {
                OpenLogFile();
            }

            try
            {
                _logWriter?.WriteLine($"{DateTime.Now:HH:mm:ss.fff dd/MM/yyyy} ***** OPEN LOG *****");
                _logWriter?.WriteLine("");
            }
            catch
            {
            }
        }
    }

    public static void CloseLog(string logText = "")
    {
        lock (_lockObj)
        {
            if (!_dialerLogFlag || _openCounter == 0)
                return;

            try
            {
                if (!string.IsNullOrEmpty(logText))
                    _logWriter?.WriteLine(logText);

                _logWriter?.WriteLine($"{DateTime.Now:HH:mm:ss.fff dd/MM/yyyy} ***** close log *****");
                _logWriter?.WriteLine("");
                _logWriter?.Flush();
            }
            catch
            {
            }

            if (--_openCounter == 0)
            {
                CloseLogFile();
            }
        }
    }

    private static void PrivateWrite(string logText)
    {
        if (!_dialerLogFlag && _openCounter == 0)
        {
            BackupLargeFile();
            OpenLogFile();
            _writeCounter = 0;
            _openCounter = 1;
            _dialerLogFlag = true;
        }
        else if (_dialerLogFlag && _writeCounter >= 1000)
        {
            CloseLogFile();
            BackupLargeFile();
            OpenLogFile();
            _writeCounter = 0;
        }

        try
        {
            _writeCounter++;
            _logWriter?.WriteLine($"{DateTime.Now:HH:mm:ss.fff dd/MM/yyyy}");
            _logWriter?.WriteLine(logText);
            _logWriter?.WriteLine("");
        }
        catch
        {
        }
    }

    private static void PrivateFlush()
    {
        if (_openCounter == 0 || !_dialerLogFlag)
            return;

        try
        {
            _logWriter?.Flush();
        }
        catch
        {
        }
    }

    public static void Write(string logText)
    {
        lock (_lockObj)
        {
            PrivateWrite(logText);
        }
    }

    public static void WriteException(string logText, Exception e = null)
    {
        lock (_lockObj)
        {
            PrivateWrite($"{logText}{Environment.NewLine}Exception Details: {e}");

        }
    }

    public static void Flush()
    {
        lock (_lockObj)
        {
            PrivateFlush();
        }
    }

    public static void WriteAndFlush(string logText)
    {
        lock (_lockObj)
        {
            PrivateWrite(logText);
            PrivateFlush();
        }
    }
}
