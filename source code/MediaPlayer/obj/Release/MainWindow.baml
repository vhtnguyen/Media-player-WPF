   M S B A M L   `   `   `  ���� F  BMediaPlayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:MediaPlayerMediaPlayer      MediaPlayer.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:MediaPlayer     -     � Title$  Media Player��$	��583��$
��1000��.���� 
���   �$	��0.5q�._��  � ColumnDefinitions �� $��1* ��� $��4* � � RowDefinitions �� $��6* ��� $��1* �.+��� 
���   �$��	0,0,0.5,0q�._�]� $
��Left=�$��3q�$��Stretch=�$��Stretch=� K� ZIndex$ 1��$��0��$��0��.+��� -    ��Name$ 
homeButton��$	��Top=�$	��150��$��35��$��3q�$
��Left=�$��Center=�
������ 
������ .����  �� Orientation$ 
Horizontal=�$	��130��.+��� -   $��homeButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/home.png���� -   $��homeButtonName��$
��Home��$��10,3,3,3q�$
��Bold"��� -   $ playqueueButton��$	��Top=�$	��150��$��35��$��3q�$
��Left=�$��Center=�
������ 
������ .���� $ 
Horizontal=�$	��130��.+��� -   $��playqueueButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/queue.png���� -   $��playqueueButtonName��$��
Play queue��$��10,3,3,3q��� -   $ playlistButton��$	��Top=�$	��150��$��35��$��3q�$
��Left=�$	��Top=�
������ 
������ .���� $ 
Horizontal=�$	��130��.+��� -	   $��playlistButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/list.png���� -
   $��playlistButtonName��$��Playlist��$��10,3,3,3q�]� $��0��$��1��$��Stretch=�$��Stretch=�.+��� -   $��HomePage��$y�Visible=�$��20q�.+��� $
��Home��$��10q�$��	ExtraBold"�$��20(��� .+��� -   $��panel_nowplaying��$y�	Collapsed=�.+�� 
������ $��
10,10,10,5q�.+��� $��Now Playing��$��	ExtraBold"�$��15(��� -   
������ 
������ .���� $��16��$��15��$
��Fill=�$��3q�$��/img/continue.png��� $��25,0,5,5q�.+��� $��Track: �� �� Padding$ 3q�$��10(��� $��5,0,5,5q�$ 3q�$��Italic$�$��10(����� *	VideoName+� 
������ $��
10,10,10,5q�.+��� $��Recent Play��$��	ExtraBold"�$��15(��� -   
������ 
������ .���� $��16��$��15��$
��Fill=�$��3q�$��/img/continue.png���� -   $��listrecentplay��$	��280��$	��Top=�$��Center=�$��10,5,0,0q�
������ $��Disabled=� �� SelectionMode$ Extended=�����  �� 
TargetType" ��.��� 	 �� Property	 � |�
 �� Value
 .  �� 	 �@J�IsHitTestVisible
 .  �� 	 � ��$
 Stretch=����� .R�� ���� .R��� $ 
Horizontal=�$��20��$	��600��.+�� $��Center=�.+��� $��10��$��10��$
��Fill=�$��3q�$��/img/videolist.png���� $��13(�$��10,0,0,0q�$��Normal$����� *	VideoName+�� -   $��PlaylistPage��$y�	Collapsed=�.+��� $��Playlist��$��10q�$��	ExtraBold"�$��20(�� $��20,5,0,0q�.+��� -   $ addplaylistButton��$��33��$��3q�$
��Left=�
������ $��2q�
���"�"�.���� $ 
Horizontal=�.+��� -   $��addplaylistButtonIcon��$��14��$��14��$
��Fill=�$��3q�$��/img/add.png���� -   $��addplaylistButtonName��$��New Playlist��$��3q��� -   $��viewPlaylist��$��33��$��15,0,0,0q�$��3q�$
��Left=�
������ $��2q�
���"�"�.���� $ 
Horizontal=�.+��� $��14��$��14��$
��Fill=�$��3q�$��/img/continue.png���� $��View Playlist��$��3q��� -   $��listPlaylist��$	��500��$	��300��$	��Top=�$��Center=�$��	20,10,0,0q�
������ $��Disabled=� � 	Resources    listViewContextMenu m� Icon��%H   &	        �� .@�m� -   $��Delete�� �� $��/img/remove.png������ " ��.��� 	 �@�ContextMenu#
 [  ���� .R�� ���� .R��� $ 
Horizontal=�$��30��$	��450��.+�� $��Center=�.+��� $��14��$��14��$
��Fill=�$��3q�$��/img/playlist.png���� $��15(�$��10,0,0,0q�$��Normal$����� *PlaylistName+�� -   $��PlaylistInforPage��$y�	Collapsed=�.+�� .+��� $��
Playlist: ��$��
10,10,0,10q�$��	ExtraBold"�$��15(��� $��	5,10,0,10q�$
��Bold"�$��Italic$�$��15(�
���"�"����� *PlaylistName+� $��20,5,0,5q�.+��� -   $ playListButton��$��70��$��33��$��3q�$��15,0,0,0q�$
��Left=�
������ $��1q�
���"�"�.���� $ 
Horizontal=�.+��� -   $��playListButtonIcon��$��14��$��14��$
��Fill=�$��3q�$��/img/play.png���� -   $��playButtonName��$
��Play��$��3q��� -   $ addsongButton��$��70��$��33��$��3q�$��15,0,0,0q�$
��Left=�
������ $��1q�
���"�"�.���� $ 
Horizontal=�.+��� -   $��addsongButtonIcon��$��14��$��14��$
��Fill=�$��3q�$��/img/add.png���� -   $��addsongButtonName��$��Add to��$��3q��� -   $ deleteButton��$��70��$��33��$��3q�$��15,0,0,0q�$
��Left=�
������ $��1q�
���"�"�.���� $ 
Horizontal=�.+��� -   $��deleteButtonIcon��$��14��$��14��$
��Fill=�$��3q�$��/img/remove.png���� -    $��deleteButtonName��$��Delete��$��3q��� -!   $ saveplaylistButton��$��70��$��33��$��3q�$��15,0,0,0q�$
��Left=�
������ $��1q�
���"�"�.���� $ 
Horizontal=�.+��� -"   $��saveplaylistButtonIcon��$��14��$��14��$
��Fill=�$��3q�$��/img/save.png���� -#   $��saveplaylisteButtonName��$
��Save��$��3q��� -$   $��	listVideo��$	��500��$	��300��$	��Top=�$��Center=�$��	10,10,0,0q�
������ $��Disabled=� ��%H   &	        �� .@�m� -%   $��Remove�� �� $��/img/remove.png������ " ��.��� 	 �@�ContextMenu#
 [  ���� .R�� ���� .R��� $ 
Horizontal=�$��20��$	��450��.+�� $��Center=�.+��� $��10��$��10��$
��Fill=�$��3q�$��/img/videolist.png���� $��13(�$��10,0,0,0q�$��Normal$����� *	VideoName+]� -&   $��PlayqueuePage��$y�	Collapsed=�$��
5,10,20,10q�.+��� $	��Top=�$��
Play Queue��$��	ExtraBold"�$��20(�$��15,0,0,0q��� -'   $��queueHeader��$ 
Horizontal=�$	��Top=�$��	15,0,0,10q�.+��� -(   $��videoStatus��$��	Playing: ��$��0,5,0,0q�$��12(�$
��Bold"�
���"�"��� -)    ��Name$ 	videoname��$��No track selected��$��5,5,10,0q�$��12(�$��Italic$�]� .+��� $��Right=�
���   �$	��0.5q� �� Padding$ 	10,5,10,5q�._�]� -*   $��	queueList��$	��200��.+��� $ 
Horizontal=�$	��Top=�$��10,0,0,0q�.+��� $��	Playlist ��$��Normal"�$��Italic$�$��14(��� $
��Wrap=�$
��Bold"�$��Italic$�$��14(�
����d����� *PlaylistName+�� $	��Top=�
���   �$	��0.5q�$��0,5,0,0q��� -+   $��listPlayqueue��$
��1000��$	��Top=�$��Right=�$��0,5,0,0q�
������ $��Disabled=�$	��Top=�$
��Auto=�$ Extended=����� " ��.��� 	 � |�
 .  �� 	 �@J�IsHitTestVisible
 .  ���� .R�� ���� .R��� $ 
Horizontal=�$��20��.+�� $��Center=�.+��� $��10��$��10��$
��Fill=�$��3q�$��/img/videolist.png�� �� BitmapScalingMode$ HighQuality=�y��� *displayMode+�� $	��180��$��WordEllipsis=�$��13(�$��10,0,0,0q�$��Normal$����� *	VideoName+�� $
��Left=�$��Center=�$��Center=�.+�r� -,   $��videoplayer��$y�	Collapsed=�$	��Top=� r� LoadedBehavior$ Manual=�$	��500��$	��360���� --   $��mp3bg��$y�Visible=�$	��100��$��3q�$��/img/music.png���� 
���   �$��	0,0.5,0,0q�$��0,0,0,10q�$��2��$��1��._���  �� Bottom$ 0��$ 2��$��Center=�.+�� $��Center=�$��Center=�$��0q�.+��� $��Center=�$��Center=�$ 
Horizontal=�.+��� $��Bottom=�$ 
Horizontal=�$��Center=�.+��� -.   $ 
prevButton��$��30��$��2q�$
��Left=�$��Center=�
������ .���� $ 
Horizontal=�.+��� -/   $��prevButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/previous.png���� -0   $��prevButtonText��$��Previous��$��3q�
���q�<�$��Black"��� -1   $ 
shufButton��$��30��$��2q�$
��Left=�$��Center=�
������ $��20,0,0,0q�.���� $ 
Horizontal=�.+��� -2   $��shufButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/shuffle.png���� -3   $��shufButtonText��$��Shuffle: Off��$��3q�
���<��$��Black"��� -4   $ playvideoButton��$��30��$��70��$��2q�$
��Left=�$��Center=�
������ $��20,0,0,0q�.���� $ 
Horizontal=�.+��� -5   $��playvideoButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/pause.png���� -6   $��playvideoButtonText��$��Pause��$��3q�
����d�$��Black"��� -7   $ pauseButton��$��30��$��70��$��2q�$
��Left=�$��Center=�
������ $��20,0,0,0q�.���� $ 
Horizontal=�.+��� -8   $��stopButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/stop.png���� $
��Stop��$��3q�
��� E��$��Black"��� -9   $ 
nextButton��$��30��$��70��$��2q�$
��Left=�$��Center=�
������ $
��20,0q�.���� $ 
Horizontal=�.+��� -:   $��nextButtonIcon��$��16��$��15��$
��Fill=�$��3q�$��/img/next.png���� $
��Next��$��3q�
���q�<�$��Black"��� $ 
Horizontal=�$��Center=�$��Center=�$
��10,5q�$	��300��.+��� -;   $ currentPosition��$
��0:00���� -<    ��Name$ progressSlider�� �� IsMoveToPointEnabled . $	��200��$��20��$
��10,0q�$��Center=�$��Center=��� -=   $ totalPosition��$
��0:00��