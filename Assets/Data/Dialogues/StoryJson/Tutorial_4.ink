-> Tutorial_4
    
=== Tutorial_4
#actor MoMo
#UIMode Out
자네 잠깐 나 좀 보세나
*[다음] -> Conversation1

=== Conversation1
#actor Pago
저... 저요?
*[다음] -> Conversation2

=== Conversation2
#actor MoMo
그래 파고 자네 말일세.
*[다음] -> Conversation3

=== Conversation3
#actor Pago
무.. 무슨 일이신가요?
*[다음] -> Conversation4

=== Conversation4
#actor MoMo
자네 남는 구리가 좀 있는가?
*[다음] -> Conversation5

=== Conversation5
#actor Pago
네, 있어요.
*[다음] -> Conversation6

=== Conversation6
#actor MoMo
있으면 조금 가져다 주게 좋은 가격에 값을 치뤄 주지.
*[다음] -> Conversation7

=== Conversation7
#actor Pago
저야 좋죠. 얼만큼 필요하신가요?
*[다음] -> Conversation8

=== Conversation8
#actor MoMo
음... 지금은 일단 다섯 개 정도면 충분할 것 같아.
물론, 그보다 더 많아도 상관 없네.
*[다음] -> Accept

=== Accept
#actor Pago
#quest tutorial_4
다섯 개 말이시죠? 알았어요.
금방 가져다 드릴게요.
    -> END
    
    
    