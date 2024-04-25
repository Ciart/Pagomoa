-> 시작


 === 시작
#talker 농
#sprite noungNounsense1
 #UIMode Out
이봐, 부딪혔잖아!
* [응, 그런데?] -> chat0_1
* [아얏..] -> chat0_1


=== chat0_1
#talker 농
#sprite noungNounsense2
사과 안 해?
* [사과한다] -> chat0_1_sorry
* [화낸다] -> chat0_1_angry


=== chat0_1_sorry
#talker 나
#sprite Idle_2
정말 미안해
*[다음] -> chat0_1_sorry_accept


=== chat0_1_sorry_accept
#talker 농
#sprite noungIdle1
뭐, 미안하면 됐어.
-> END


=== chat0_1_angry
#talker 나
#sprite Idle_4
내가 왜? 너야말로 사과하시지!
*[다음] -> chat0_1_angry_end


=== chat0_1_angry_end
#talker 농
#sprite noungSad
후.. 요즘 애들이란..
-> END
