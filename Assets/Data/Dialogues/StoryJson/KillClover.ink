-> KillClover

=== KillClover
#talker 농
#sprite noungNounsense1
#UIMode Out
어라? 너 혹시 지난번에 나한테 광물을 건네줬던?
*[모르는 척 한다] -> ENDStory
*[반갑게 마주한다] -> Story1

=== ENDStory
#talker 농
#sprite noungNounsense1
어... 야...! 어디가! 왜 무시해!
-> END

=== Story1
#talker 나
#sprite Idle_2
어.. 맞아, 지난번에 준 랜턴은 잘 쓰고 있어!
*[다음] -> Story2

===Story2
#talker 농
#sprite noungNounsense1
요즘에 클로버 쥐들이 마구 작물을 갉아먹지 뭐야
*[다음] -> Story3

===Story3
그래서 그런데 10마리만 잡아줄 수 있어?
*[수락] -> Accept
*[거절] -> Deny
 
  === Accept
 #quest 2
 흐흐..! 좋아 그 중에 설마 네잎클로버 하나 없겠어?
 -> END
 
 === Deny
쳇! 아쉽구만. 잘 먹고 잘 살라구
 -> END