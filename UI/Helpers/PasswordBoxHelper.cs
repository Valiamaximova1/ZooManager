using System.Windows;
using System.Windows.Controls;

namespace UI.Helpers
{
    public static class PasswordBoxHelper
    {
        //видимата парола 
        //това ще позволи да си взема данните чрез Binding
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));
        //включване на логиката
        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));
        //при ръчно сменяне на паролата
        private static readonly DependencyProperty UpdatingPasswordProperty =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxHelper));

        public static string GetBoundPassword(DependencyObject dp) => (string)dp.GetValue(BoundPasswordProperty);
        public static void SetBoundPassword(DependencyObject dp, string value) => dp.SetValue(BoundPasswordProperty, value);

        public static bool GetBindPassword(DependencyObject dp) => (bool)dp.GetValue(BindPasswordProperty);
        public static void SetBindPassword(DependencyObject dp, bool value) => dp.SetValue(BindPasswordProperty, value);

        private static bool GetUpdatingPassword(DependencyObject dp) => (bool)dp.GetValue(UpdatingPasswordProperty);
        private static void SetUpdatingPassword(DependencyObject dp, bool value) => dp.SetValue(UpdatingPasswordProperty, value);

        //като се променя паролата от view model
        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox && !GetUpdatingPassword(passwordBox))
            {
                //махаме стария евент
                passwordBox.PasswordChanged -= HandlePasswordChanged;
                //слагаме новата парола в password box
                passwordBox.Password = (string)e.NewValue ?? string.Empty;
                //добавяме евент лисънър
                passwordBox.PasswordChanged += HandlePasswordChanged;
            }
        }
        //BindPassword="True"
        //добавяме или премахваме PasswordChanged събитието,
        //което слуша кога потребителят въвежда нещо в PasswordBox.
        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox passwordBox)
            {
                bool wasBound = (bool)e.OldValue;
                bool needToBind = (bool)e.NewValue;

                if (wasBound)
                    passwordBox.PasswordChanged -= HandlePasswordChanged;

                if (needToBind)
                    passwordBox.PasswordChanged += HandlePasswordChanged;
            }
        }
        //като пишеш паролата
        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            //включва флага UpdatingPassword = true
            SetUpdatingPassword(passwordBox, true);
            //Взима стойността на PasswordBox.Password
            SetBoundPassword(passwordBox, passwordBox.Password);
            //Задава я в BoundPassword( във ViewModel)
            SetUpdatingPassword(passwordBox, false);
        }
    }
}
