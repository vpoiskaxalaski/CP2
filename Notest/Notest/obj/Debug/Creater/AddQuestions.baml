   M S B A M L   `   `   `  ����A  =Notest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NotestNotest  5   	       Notest.AddQuestions   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	    localclr-namespace:Notest  5   	   $	��400��5	   	   $	��300��6     � Icon$  ..\ico\main.ico��6$    � WindowStyle$
 None=�5
   	    � AllowsTransparency . ��5      �� 5   
    �� ImageSource$ ..\picture\6.jpg��6   .��5      �  � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      .+�5   
   � $��0��6   ���#7a6e6c.+�5      o� -   5        $��	Languages��5      $
��Left=�5      ��5      Z� .@�5      m� -   5        $��Русский��5      $��Center=�6C   m� 5      -   5        $��English��5      $��Center=�6C   5      �� 5      -   5        $��43��5      $
��Auto��6    $��Right=�5      ��5      Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ WindowClose=�6*   $��27��5    .   $��21��69   $��Center=�5!   .   $��Center=�5"   .   5$      5%      �� 5&   
   $��1��6   $��Hidden=�5'      $��	0,10,0,55q�5(      .��5)      �� -   5        $��QuestionList��5)      $��1��6/   ��5*      �� 5+      .R�5,      ��  ��Name$ Border��6!    �� Padding$ 8q�61   $��0 0 0 1q�5-         MaterialDesignDivider#���   5.      ._�5/      �  � ColumnDefinitions 50   "   �� 51   &   $��60 �67   	 z� SharedSizeGroup$	 Checkerz��52   %   �� 53   &   $	��190 �68   .+�55   "   d� -   5        $��Center=�55   /    ) %MaterialDesignActionLightToggleButton#��[ 56   3   
 d� 	IsChecked
 55   J   �� *
IsSelected+��57   3   �� *Code+
 � Tag 58   3   �� 59   3   �� 5:   "   $��8 0 0 0q�6-   $��1��6>   .+�5;   &   �� $
��Bold"�60   $
��Wrap=�5=   ,    5<   -   �� ��5>   ,   �� *	Question1+5?   #   5A      5C       �� Triggers 5E      �� 5G       �� Value$
 True��6I    �� Binding 6*   �� *
IsSelected+.a�5I   "   ��  �� 
TargetName$ Border��6)     MaterialDesignSelection �� Value# �  6S    �� Property � ��5K      5O      5S      5T      �� 5U      -   5        $��FindTestButton��5V      $��1��5U      $��80��5W      $��25��6   ���#7a6e6c$��Right=�5Y      $��Bottom=�5Z      $��25q�5[      ���#e3e0de 
 select#���  5]      $��10(�5^      5_      5`      5a      