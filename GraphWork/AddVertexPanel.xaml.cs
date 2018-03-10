using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphWork
{
    /// <summary>
    /// Логика взаимодействия для AddVertexPanel.xaml
    /// </summary>
    public partial class AddVertexPanel : UserControl
    {
        /*TODO:
         формы для ввода и редактирования свойств вершин будут отображаться вот на таких вот панельках,
         которые будут содержаться во всплывающей снизу области окна (анимации каеф)
         еще будет отдельное окно где будет выводиться выбранная матрица тупо в текстбокс
         но это все потом щас надо сюда припилить объектную модель вирта
         сначала потестить работает ли она
             */
        public AddVertexPanel()
        {
            InitializeComponent();
        }
    }
}
