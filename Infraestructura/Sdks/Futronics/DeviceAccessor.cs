//using System;

//namespace Futronic.Devices.FS88
//{
//    public class DeviceAccessor
//    {
//        public FS88 AccessFingerprintDevice()
//        {
//            var handle = LibScanApi.ftrScanOpenDevice();

//            if (handle != IntPtr.Zero)
//            {
//                return new FS88(handle);
//            }

//            throw new Exception("Cannot open device");
//        }

//        // public CardReader AccessCardReader()
//        // {
//        //     var handle = LibMifareApi.ftrMFOpenDevice();

//        //     if (handle != IntPtr.Zero)
//        //     {
//        //         return new CardReader(handle);
//        //     }

//        //     throw new Exception("Cannot open device");
//        // }
//    }
//}