   M S B A M L   `   `   `  ����G  CViewModelLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ViewModelLibViewModelLib  5   	   Q-clr-namespace:ViewModelLib.AttachedPropertiesViewModelLib.AttachedProperties  5   	   '    !ViewModelLib.IntegerTextBoxDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ,localclr-namespace:ViewModelLib     5   	   LattachedProperties-clr-namespace:ViewModelLib.AttachedProperties     5   	   
���   �  � 
ResizeMode$  NoResize=�5   	   $��48��5   	   $��29��5   	    � WindowStyle$
 None=�5   	    � ShowInTaskbar .   +� FocusedElement 5   	   ��  �� ElementName$ IntegerTextBox��
 � Top 5	   	   ��  �� Path$ 	YPosition� �� Mode$ TwoWay=� � Left 5
   	   �� $ 	XPosition�$ TwoWay=�	 � 	Resources	 5      
 �� MergedDictionaries �� Source!   ViewModelLib.IntegerTextBox �� 
TargetType �� BasedOn �� Triggers P� Property P� Value �� Property �� Value �� RelativeSource�� 5   
   
 5      �� 5      $ MyStyleDictionary.xaml?�6%   %�   '        7���� 5      "  6   8    5      P� 5       �@=�HasError . . �5      ��  �@�ToolTip 64   ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent�5      5      5      .��5"      �� -   5        $��
MainCanvas��5"      
���   �.+�5$   
   �� 
���i��$��1q�6)   
���   � �� CornerRadius$ 1��6P   $��3,4,3,0q�6a   ._�5%        -   5          Name$ IntegerTextBox��5%   #   
���   �
���i��$��0q�5'   M   
���i��$��30��5(   8   $��30��6F      MaxNumberIntegers$ 3��6Q    �� SelectionBrush
 �i����5&   !   �� $ TextBoxText� �� ValidatesOnDataErrors .  �� UpdateSourceTrigger$ PropertyChanged=�$ TwoWay=� �� BindsDirectlyToSource . .��5)   <   5*      5+      �� 5-   
   
 �� Top$ 0��6    �� Right$ 0��6       CloseButtonStyle#��[  61    �� IsCancel . 
���i��$��1,0,3,1q�5.      $��Verdana)�5/      $��10(�6'    � ToolTip$ close without saving��65   ��50      �� *CloseNoSaveCommand+�� 52   
    �� Bottom$ 0��6   $ 0��6#     CloseSaveButtonStyle#��[ 64   $ close and save��6b   
���i��$��3q�53      $��10(�6     �� 	IsDefault  . ��54      �� $ 	OkCommand�55      57      