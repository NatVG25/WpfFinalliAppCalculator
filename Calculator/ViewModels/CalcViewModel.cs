using Calculator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator.ViewModels
{
    public class CalcViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Constructor
        
        public CalcViewModel() //конструктор класса  CalcViewModel
        {
            Result = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation);
            ClearDisplayCommand = new RelayCommand(ClearDisplay);
            ResetCommand = new RelayCommand(Reset);
            GetResultCommand = new RelayCommand(GetResult);
        }
        #endregion

        #region Commands

        public ICommand AddNumberCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand ClearDisplayCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand GetResultCommand { get; set; }

        private void AddNumber(object obj) //метод вывода чисел
        {
            var number = obj as string;
            if (Result == "0" && number != ",")
                Result = string.Empty;
            else if (number == "," && isOperationRequested)
                number = "0,";

            Result += number;
        }

        private void AddOperation(object obj)  //метод вывода знаков мат.операций
        {
            var operation = obj as string;
            if (isOperationRequested)
                Result = Result.Substring(0, Result.Length - 1);

            Result += operation;
        }

        private void GetResult(object obj)  //метод вывода результата
        {
            var result = dataTable.Compute(Result.Replace(",", ".").Replace("x", "*"), "");
            var resultToDisplay = Math.Round(Convert.ToDouble(result), 15);
            Result = resultToDisplay.ToString();
        }

        private void ClearDisplay(object obj) //метод очистки display
        {
            Result = "0";
        }

        private void Reset(object obj) //метод удаления последнего знака
        {
            if (Result.Length > 1)
                Result = Result.Substring(0, Result.Length - 1);
            else if (Result.Length == 1)
                Result = "0";
            else
                Result = "0";
        }

        #endregion

        #region Properties
        private string result; //закрытое поле result

        public string Result  //открытое свойство Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        private bool isOperationRequested //проверка допустимости выполнения операции, выражениедолжно содержать один из знаков из списка
        {
            get { return availableOperations.Contains(Result.Substring(Result.Length - 1)); }
        }

        private List<string> availableOperations = new List<string>() { "/", "x", "-", "+" };
        
        private DataTable dataTable = new DataTable();
        #endregion

    }
}
