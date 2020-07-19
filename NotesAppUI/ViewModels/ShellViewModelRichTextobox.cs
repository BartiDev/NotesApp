using Caliburn.Micro;
using NotesAppDataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace NotesAppUI.ViewModels
{
    public partial class ShellViewModel : Screen, IShell, IHandle<UserModel>, IHandle<string>
    {

        private int _selectedFontSize = 12;
        private FontFamily _selectedFontFamily = new FontFamily("Cambria");
        private List<int> _fontSizes = new List<int> { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26, 28, 32, 36, 40, 44, 48, 54, 60, 66, 72, 80, 88, 96 };

        private bool _isBolded;
        private bool _isItalic;
        private bool _isUnderlined;


        public List<int> FontSizes 
        { 
            get { return _fontSizes; } 
        }
        public int SelectedFontSize
        {
            get { return _selectedFontSize; }
            set { _selectedFontSize = value; NotifyOfPropertyChange(nameof(SelectedFontSize)); }
        }
        public FontFamily SelectedFontFamily
        {
            get { return _selectedFontFamily; }
            set { _selectedFontFamily = value; NotifyOfPropertyChange(nameof(SelectedFontFamily)); }
        }

        public bool IsBolded
        {
            get { return _isBolded; }
            set { _isBolded = value; NotifyOfPropertyChange(nameof(IsBolded)); }
        }
        public bool IsItalic
        {
            get { return _isItalic; }
            set { _isItalic = value; NotifyOfPropertyChange(nameof(IsItalic)); }
        }

        public bool IsUnderlined
        {
            get { return _isUnderlined; }
            set { _isUnderlined = value; NotifyOfPropertyChange(nameof(IsUnderlined)); }
        }


        public void RBTextChanged(object o)
        {
            ((RichTextBox)o).Selection.ApplyPropertyValue(TextElement.FontSizeProperty, SelectedFontSize.ToString());
            ((RichTextBox)o).Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, SelectedFontFamily);
            if(IsBolded)
                ((RichTextBox)o).Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                ((RichTextBox)o).Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            if (IsItalic)
                ((RichTextBox)o).Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                ((RichTextBox)o).Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            if (IsUnderlined)
                ((RichTextBox)o).Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
            {
                TextDecorationCollection textDecorations;
                (((RichTextBox)o).Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                ((RichTextBox)o).Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }
    }
}
