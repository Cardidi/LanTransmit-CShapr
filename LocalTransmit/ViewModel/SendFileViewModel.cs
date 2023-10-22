using System.ComponentModel;

namespace LocalTransmit.ViewModel
{
    public class SendFileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_isSending;

        public bool IsSending
        {
            get => m_isSending;
            set
            {
                m_isSending = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(m_isSending)));
            }
        }
    }
}
