using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model
{
    public class BP4DeviceLoginModel
    {
        private string _address;
        private int _addressType;
        private string _cmdPort;
        private string _dataPort;
        private string _deviceType;
        private string _userName;
        private string _password;

        public string Password
        {
            get 
            {  if (_password == null || _password == "")
                {
                    _password = "123456";
                }
                return _password;
            }
            set
            {

               _password = value;
            }
        }

        public string UserName
        {
            get
            {
                if (_userName == null || _userName == "")
                {
                    _userName = "admin";
                }
                return _userName;
            }
            set
            {

                _userName = value;
            }
        }

        public string DeviceType
        {
            get 
            {
                if (_deviceType == null || _deviceType == "")
                {
                    _deviceType = "Bsr.LimitDevice";
                }
                return _deviceType;
            }
            set
            {
                  _deviceType = value;
            }
        }

        public string DataPort
        {
            get 
            {
                if (_dataPort == null || _dataPort=="")
                {
                    _dataPort = "3720";
                }
                return _dataPort;
            }
            set
            {
                _dataPort = value;
            }
        }

        public string CmdPort
        {
            get 
            {
                if (_cmdPort == null ||_cmdPort=="")
                {
                    _cmdPort = "3721";
                }
                return _cmdPort; 
            }
            set 
            {
                _cmdPort = value;
            }
        }

        public int AddressType
        {
            get
            {
                return _addressType;
            }
            set
            {
                _addressType = value;
            }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

    }
}
