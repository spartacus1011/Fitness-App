   M S B A M L   `   `   `  ����K  GExercise tracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Exercise_tracker.ViewsExercise_tracker.Views  5      7    1Exercise_tracker.Views.ExerciseRosterListViewItem   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:Exercise_tracker.Views     5        � 	Resources  5
         	BoolToVis��%   &	        �� 5   
   6'   5      .��5      � $
��auto��6   ��5   
   �� 5       �� 
TargetType" �6   .�5      ��  �� Property � �� �� Value �	#FF54B8666C    �� Triggers 5      �� 5       �� Binding 6"   �� *IsUsedInRoster+ �� Value	 false6F   .a�5      ��  � �� �	#FF6666666K   5      5      .+�5   
   �� $
��Left=�6   $��Center=�6,   $��25,0,0,0q�6G   $��30��6Z   $��30��6f   ��6q   �� *ToggleAddToRosterCommand+�� 5   
   $
��Left=�6�   $��Center=�6�   $
��Wrap=�6�   $��70,0,0,0q�6�   
���������6   �� *RequiredSetsCount+ �� StringFormat$ {0} Sets��y�6P   �� *IsSets+ �� 	Converter# [  6�   �� 5   
   $��Center=�62   $	��Top=�6O   $��0,10,0,0q�6g   $
��Wrap=�6y   $��25(�6�   
�������$��Calibri)�6�   ��6   �� *ExerciseName+6�   �� 5    
   $��Center=�60   $	��Top=�6M   $��	0,58,0,10q�6e   $
��Wrap=�6x   $��20(�6�   
�������$��Calibri)�6�   ��6   �� *
ShownCount+6�   5"      5$      