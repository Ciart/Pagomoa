-> Tutorial_2_Complete

=== Tutorial_2_Complete
#actor Pago
#UIMode Out
... 자! 여기있어
*[다음] -> Conversation1

=== Conversation1
#actor Moa
배고파서 쓰러지는 줄 알았네
*[다음] -> Conversation2

=== Conversation2
#actor Moa
힘이 떨어지면 아무것도 못한단 말이야!
*[다음] -> Conversation3

=== Conversation3
#actor Pago
(모아가 구리를 먹을때마다
엄청난 소리가 난다)
*[수락] -> Accept

=== Accept
# reward tutorial_2
(모아가 먹을 광물을
챙겨 두어야겠다)

-> END
