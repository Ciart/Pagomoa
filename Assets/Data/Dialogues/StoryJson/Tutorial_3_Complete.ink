-> Tutorial_3_Complete
    
=== Tutorial_3_Complete
#actor Pago
#UIMode Out
이걸로 손을 좀 보면..
*[다음] -> Conversation1

=== Conversation1
#actor Pago
(뚝딱 뚝딱)
*[다음] -> Conversation2

=== Conversation2
#actor Moa
처음이랑 비슷한데?
*[다음] -> Conversation3

=== Conversation3
#actor Pago
이정도면 되겠다!
*[수락] -> Accept

=== Accept
#actor Pago
# reward tutorial_3
다시 보금자리를 되찾았다!
    -> END
