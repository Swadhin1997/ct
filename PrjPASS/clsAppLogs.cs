using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace PrjPASS
{
    public class clsAppLogs
    {
        public static void LogException(Exception ex)
        {
            _readWriteLock.EnterWriteLock();

            string buildPath = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/ErrorLog"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/ErrorLog");
            }

            string logFile = AppDomain.CurrentDomain.BaseDirectory + "/ErrorLog/Err_" + buildPath + ".txt";
            Mutex mutex = new Mutex(false, logFile.Replace("\\", ""));

            try
            {

                mutex.WaitOne();
                File.AppendAllText(logFile, string.Format("********** {0} **********", DateTime.Now.ToString()) + "\n" + Environment.NewLine);

                // Open the log file for append and write the log

                if (ex.InnerException != null)
                {
                    File.AppendAllText(logFile, "Inner Exception Type: " + "\n" + Environment.NewLine);
                    File.AppendAllText(logFile, ex.InnerException.GetType().ToString() + "\n" + Environment.NewLine);
                    File.AppendAllText(logFile, "Inner Exception: " + "\n" + "" + Environment.NewLine);
                    File.AppendAllText(logFile, ex.InnerException.Message + "\n" + "" + Environment.NewLine);
                    File.AppendAllText(logFile, "Inner Source: " + "\n" + Environment.NewLine);
                    File.AppendAllText(logFile, ex.InnerException.Source + "\n" + Environment.NewLine);
                    if (ex.InnerException.StackTrace != null)
                    {
                        File.AppendAllText(logFile, "Inner Stack Trace: " + "\n" + Environment.NewLine);
                        File.AppendAllText(logFile, ex.InnerException.StackTrace + "\n" + Environment.NewLine);
                        if (ex.StackTrace != null)
                        {
                            File.AppendAllText(logFile, ex.StackTrace + "\n" + Environment.NewLine);
                        }
                    }
                }
                File.AppendAllText(logFile, "Exception Type: " + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, ex.GetType().ToString() + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, "Exception: " + ex.Message + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, "Stack Trace: " + "\n" + Environment.NewLine);
                if (ex.StackTrace != null)
                {
                    File.AppendAllText(logFile, ex.StackTrace + "\n" + Environment.NewLine);
                }

                File.AppendAllText(logFile, "==========================================================" + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, Environment.NewLine);
            }
            catch (Exception)
            {

            }
            finally
            {
                mutex.ReleaseMutex();
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public static void LogEvent(string Message)
        {
            _readWriteLock.EnterWriteLock();

            string buildPath = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/EventLog"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/EventLog");
            }

            string logFile = AppDomain.CurrentDomain.BaseDirectory + "/EventLog/Evnt_" + buildPath + ".txt";
            Mutex mutex = new Mutex(false, logFile.Replace("\\", ""));
            try
            {
                mutex.WaitOne();

                // Open the log file for append and write the log
                File.AppendAllText(logFile, string.Format("********** {0} **********", DateTime.Now.ToString()) + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, "==============================================" + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, Message + "\n" + Environment.NewLine);
                File.AppendAllText(logFile, "==============================================" + "\n" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                mutex.ReleaseMutex();
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

    }
}
