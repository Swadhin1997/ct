using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace PrjPASS
{
    /// <summary>
    /// Summary description for ExceptionUtility
    /// </summary>
    public sealed class ExceptionUtility
    {
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public ExceptionUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static void LogException(Exception ex, string source)
        {
            _readWriteLock.EnterWriteLock();

            string buildPath = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/ErrorLog"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/App_Data/ErrorLog");
            }

            string logFile = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/ErrorLog/Err_" + buildPath + ".txt";
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
            catch (Exception exc)
            {

            }
            finally
            {
                mutex.ReleaseMutex();
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
        }

        //SR79011 - Implementation of new credit score service in PASS application
        //below function implemented while working on SR79011 - HASMUKH
        public static void LogEvent(string Message)
        {
            if (ConfigurationManager.AppSettings["IsWriteLogs"].ToString() == "1")
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
                    mutex.ReleaseMutex();
                    _readWriteLock.ExitWriteLock();
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
}