   M S B A M L   `   `   `  ����A  =Notest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NotestNotest  5   	       Notest.TestResults   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
    localclr-namespace:Notest  5   	     � WindowStyle$
  None=�5	   	    � AllowsTransparency .  � Title$ Notest��5
   	   $	��350��6   $	��620��6%    � Icon$ ../ico/main.ico��61    � 	Resources 5         ds �� 
TargetType �� Property �� Value  OpenDoor �� Triggers��%�   &	        &	 L     �� 5   
   " ��6   .�5      ��  � ��$ Center=�64   5      �� 5   
    5      :� 5      5      5      .��5      (� 	 (� Stretch$
	 Fill=�6   .��5   
   � 
 � RowDefinitions
 5      �� 5      $��25 �6    �� 5      $	��250 �6    .+�5      � $��0��6   ���#7a6e6c.+�5      �� -   5        $��45��5      $
��Auto��6$   $��Right=�5      $��0 0 80 0q�5      ��5      Z� ��5       Z� .��5#      �� $��..\ico\minimize.png��6   $��25��5$   .   $��25��69   $��Center=�5%   .   $��0 -7 0 0q�5&   .   5(      �� 5)      -   5        $��
Fullscreen��5)      $��45��5*      $
��Auto��6$   $��Right=�5+      $��0 0 40 0q�5,      ��5-      Z� ��5.      Z� .��50      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ 
Fullscreen=�6.   $��25��51   .   $��20��69   $��Center=�52   .   53      �� 54      -   5        $��FullscreenExit��54      $��45��55      $
��Auto��6$   $��Right=�56      $��0 0 40 0q�57      $y�Hidden=�58      ��59      Z� ��5:      Z� .��5<        $ FullscreenExit=�6.   $��25��5=   .   $��20��69   $��Center=�5>   .   5?      �� 5@      -   5        $��43��5@      $
��Auto��6$   $��Right=�5A      ��5B      Z� ��5C      Z� .��5E        $ WindowClose=�6.   $��27��5F   .   $��21��69   $��Center=�5G   .   $��Center=�5H   .   5J      5K      &   System.Windows.Controls.DataGrid  5L      -   5        $��
ResultGrid��5P      $��2��5L      $	��780��5N         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5O         AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .     AlternatingRowBackground �#a3a6af"   VerticalScrollBarVisibility$
 Auto=�5T      $   HorizontalScrollBarVisibility$
 Auto=�5U         
IsReadOnly . $��Medium"�5W         SelectionMode$ Single=�5X         FrozenColumnCount$ 5��5Y      $��0,0,-1,0q�5Z         Columns 5[      0  *System.Windows.Controls.DataGridTextColumn  5\      ,  &System.Windows.Controls.DataGridColumn   Width$ * 6)    	 login   Header# �  5^   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5]   )   �� *	UserLogin+5^   )     5_       	 topic# �  6G    6)   �� *	TestTheme+6G     5`       
 header# �  6F    6)   �� *
TestName+6F     5a      $ * 6)    
 result# �  6N    63   �� *Result+6N     5b      $ * 6)     date# �  6L    63   �� *Date+6L   5d      5e      5f      5g      