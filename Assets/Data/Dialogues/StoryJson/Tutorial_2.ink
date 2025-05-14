    -> Tutorial_2
    
=== Tutorial_2
#actor Moa
#UIMode Out
이 녀석아!
언제까지 기다리게 할거야!
*[다음] -> Conversation1

=== Conversation1
#actor Moa
가지지고만 있지 말고 어서 내놔!
이러다 배고파서 죽어 버리겠어
*[다음] -> Conversation2

=== Conversation2
#actor Pago
(모아가 더 짜증 부리기 전에 주어야겠다)
*[수락] -> Accept

=== Accept
# quest tutorial_2
(슬롯에 등록하거나 
아이콘을 우 클릭해 먹일 수 있어)
    -> END
