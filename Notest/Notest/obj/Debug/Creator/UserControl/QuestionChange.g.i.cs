﻿#pragma checksum "..\..\..\..\Creator\UserControl\QuestionChange.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3A7517A4490054F7D3DF936EE84D502B55BE737C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using Notest;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Notest {
    
    
    /// <summary>
    /// QuestionChange
    /// </summary>
    public partial class QuestionChange : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox questionText;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image PictureBox;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox questionCosttxb;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddImage;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteImage;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid AnswerDtgrd;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveAnswer;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveChanges;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Clear;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Notest;component/creator/usercontrol/questionchange.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.questionText = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 2:
            this.PictureBox = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.questionCosttxb = ((System.Windows.Controls.TextBox)(target));
            
            #line 42 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
            this.questionCosttxb.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.OnQuestionCostChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AddImage = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
            this.AddImage.Click += new System.Windows.RoutedEventHandler(this.AddImage_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DeleteImage = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\..\..\Creator\UserControl\QuestionChange.xaml"
            this.DeleteImage.Click += new System.Windows.RoutedEventHandler(this.DeleteImage_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.AnswerDtgrd = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 7:
            this.RemoveAnswer = ((System.Windows.Controls.Button)(target));
            return;
            case 8:
            this.SaveChanges = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.Clear = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
