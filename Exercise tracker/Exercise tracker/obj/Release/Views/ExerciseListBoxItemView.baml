   M S B A M L   `   `   `  ���� K  GExercise tracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Exercise_tracker.ViewsExercise_tracker.Views  I)clr-namespace:Exercise_tracker.ViewModelsExercise_tracker.ViewModels  4    .Exercise_tracker.Views.ExerciseListBoxItemView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     R NMvvmDialogs, Version=4.1.1.0, Culture=neutral, PublicKeyToken=3d3b9708589bc73e8md/https://github.com/fantasticfiasco/mvvm-dialogs  0local$clr-namespace:Exercise_tracker.Views  :
viewModels)clr-namespace:Exercise_tracker.ViewModels  $  MvvmDialogs.DialogServiceViews    IsRegistered  .  � 	Resources    	BoolToVis��%   &	        �� .��� $
��auto��$��0,0,0,0q�����  �� 
TargetType" �.���  �� Property � �� �� Value �	#FF919191 �� Triggers ��  �� Binding �� *OnTime+ �� Value true.a���  � �� �	#FFF7C234��  �� *Late+ true.a���  � �� �	#FFFF4040��  �� *Early+ true.a���  � �� �	#FF54B866.+��� $
��Left=�$��Center=�$��25,0,0,0q�$��30��$��30������ *MarkExerciseCompletedCommand+�� $
��Left=�$��Center=�$
��Wrap=�$��70,0,0,0q�
����������� *RequiredSetsCount+ �� StringFormat$ {0} Sets Remaining��y��� *IsSets+	 �� 	Converter#	 [  �� $��Center=�$	��Top=�$��0,10,0,0q�$
��Wrap=�$��36(�
�������$��Calibri)����� *ExerciseName+�� $��Center=�$	��Top=�$��	0,58,0,10q�$
��Wrap=�$��22(�
�������$��Calibri)����� *
ShownCount+�� $��Right=�$��Center=�$��0,0,40,0q�$
��Wrap=�$��20(����� *DueTimeAsString+o� $��20��$��20��$��Right=�$	��Top=�$	��5,5q����	#00000000$��12(�.@�m� $��:��$��20��$��20��$��Center=�$��Center=�.@�m� $
��Edit��
 m� Command
 �� *EditThisExerciseCommand+m� $��Delete��
 �� *DeleteThisExerciseCommand+