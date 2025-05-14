-> moa_dialogue1

=== moa_dialogue1
#actor Pago
#UIMode Out
... (모아를 빤히 처다 본다)
* [다음] -> Conversation1

=== Conversation1
#actor Moa
..?
* [다음] -> Conversation2

=== Conversation2
#actor Moa
짜증나는 얼굴 저리 치우고 먹을거나 가져와!
* [다음] -> Conversation3

=== Conversation3
#actor Pago
(지금은 건들이지 말자..)
    -> END
