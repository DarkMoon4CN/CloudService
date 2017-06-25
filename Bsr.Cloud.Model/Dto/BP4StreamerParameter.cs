 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model
{
    public class BP4StreamerParameter
    {
        private string _destinationIP;
        private string _customerToken;
        private BP4DeviceLoginModel _loginModel;
        private BP4RealStreamModel _realStreamModel;
        private string _restServIp;
        private int _restServProt;
        public int RestServPort
        {
            get
            {
                if (_restServProt == 0)
                {
                    _restServProt = 8006;
                }
                return _restServProt;
            }
            set
            {
                _restServProt = value;
            }
        }
        public string RestServIp
        {
            get
            {
                return _restServIp;
            }
            set
            {
                _restServIp = value;
            }
        }

        private int _playerType;

        public int PlayerType
        {
            get { return _playerType; }
            set { _playerType = value; }
        }

        public BP4RealStreamModel RealStreamModel
        {
            get { return _realStreamModel; }
            set { _realStreamModel = value; }
        }
        public BP4DeviceLoginModel LoginModel
        {
            get { return _loginModel; }
            set { _loginModel = value; }
        }

        public string CustomerToken
        {
            get { return _customerToken; }
            set { _customerToken = value; }
        }
        public string DestinationIP
        {
            get
            {
                if (_destinationIP == null) 
                {
                    _destinationIP = "";
                }
                return _destinationIP;
            }
            set 
            { 
                _destinationIP = value;
            }
        }
    }
}
