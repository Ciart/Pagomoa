-> 시작


 === 시작
#talker 농
#sprite noungNounsense1
 #UIMode Out
어음, 할 말이 있는데.. 들어줄 수 있어?
* [응, 뭔데??] -> chat0_1
* [나 바빠. 다음에] -> chat0_2


=== chat0_1
#talker 농
#sprite noungNounsense2
사실 나는.. 드래곤이야
* [어이상실] -> chat0_1_sorry
* [화낸다] -> chat0_1_angry

=== chat0_2
#talker 농
#sprite noungNounsense2
..
-> DONE

=== chat0_1_sorry
#talker 나
#sprite Idle_2
뭐!? 개가 아니었어?
*[다음] -> chat0_1_sorry_accept


=== chat0_1_sorry_accept
#talker 농
#sprite noungIdle1
어응.. 사실 드래곤이었어..
-> END


=== chat0_1_angry
#talker 나
#sprite Idle_4
뭔 개소리야!?
*[다음] -> chat0_1_angry_end


=== chat0_1_angry_end
#talker 농
#sprite noungSad
흑.. 역시 못 믿겠지? 다들 그러더라 ㅠ
-> END