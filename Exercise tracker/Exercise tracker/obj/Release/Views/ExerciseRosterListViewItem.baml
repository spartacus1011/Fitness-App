   M S B A M L   `   `   `  ���� K  GExercise tracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Exercise_tracker.ViewsExercise_tracker.Views  7    1Exercise_tracker.Views.ExerciseRosterListViewItem   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     6local$clr-namespace:Exercise_tracker.Views       � 	Resources     	BoolToVis��%   &	        �� .��� $
��auto������  �� 
TargetType" �.���  �� Property � �� �� Value �	#FF54B866 �� Triggers ��  �� Binding �� *IsUsedInRoster+ �� Value	 false.a���  � �� �	#FF666666.+��� $
��Left=�$��Center=�$��25,0,0,0q�$��30��$��30������ *ToggleAddToRosterCommand+�� $
��Left=�$��Center=�$
��Wrap=�$��70,0,0,0q�
����������� *RequiredSetsCount+ �� StringFormat$ {0} Sets��y��� *IsSets+ �� 	Converter# [  �� $��Center=�$	��Top=�$��0,10,0,0q�$
��Wrap=�$��25(�
�������$��Calibri)����� *ExerciseName+�� $��Center=�$	��Top=�$��	0,58,0,10q�$
��Wrap=�$��20(�
�������$��Calibri)����� *
ShownCount+