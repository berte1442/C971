﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System.IO;
//using Xamarin.Forms;
//using C971.Droid;

//namespace C971.Droid
//{
//    class LocalFileHelper : ILocalFileHelper
//    {
//        public string GetLocalFilePath(string fileName)
//        {
//            string docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
//            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

//            if (!Directory.Exists(libFolder)){
//                Directory.CreateDirectory(libFolder);
//            }
//            return Path.Combine(libFolder, fileName);
//        }
//    }
//}