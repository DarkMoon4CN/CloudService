using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model
{
    public class BP4RealStreamModel
    {
        private int _channel;
        private int _subStream;
        private int _transProc;
        private int _channelId;

        public int ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }


        public int TransProc
        {
            get { return _transProc; }
            set { _transProc = value; }
        }

        public int SubStream
        {
            get { return _subStream; }
            set { _subStream = value; }
        }

        public int Channel
        {
            get 
            {
                if (_channel == 0)
                {
                    _channel = 1;
                }
                return _channel;
            }
            set 
            {
               _channel = value;
            }
        }

    }
}
