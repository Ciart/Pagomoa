-> CollectMineral

=== CollectMineral
#talker 농
#sprite noungNounsense1
#UIMode Out
아, 요즘 돈벌기가 참 어려워.. 땅을 파서 나오면 얼마나 좋아
*[...] -> Story1

=== Story1
#talker 농
#sprite noungNounsense1
어. 거기 지나가던 너! 혹시.. 엳듣기만 하는 거 아니지?
*[대답한다] -> Story2
*[무시한다] -> ENDStory

=== ENDStory
#talker 농
#sprite noungNounsense1
요즘 애들은 참..
-> END

=== Story2
#talker 나
#sprite Idle_2
음.. 땅파면 광물이 잘 나오는 것 같던데.. 파보는게 어때?
*[다음] -> Quest
 
 === Quest
 #talker 농
#sprite noungNounsense1
정말? 혹시 파본 광물이라도 있어? 있으면 두개만 구해와줄래?
*[수락] -> Accept
*[거절] -> Deny
 
  === Accept
 #quest 1
 정말 고마워! 이 은혜는 절대 잊을게
 -> END
 
 === Deny
음.. 어쩔 수 없지.. 호구잡기 실패네 쳇..
 -> END