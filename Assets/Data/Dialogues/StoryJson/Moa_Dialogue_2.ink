-> moa_dialogue2

=== moa_dialogue2
#actor Pago
#UIMode Out
모아야
* [다음] -> Conversation1

=== Conversation1
#actor Moa
왜?
* [다음] -> Conversation2

=== Conversation2
#actor Pago
넌 내가 어디 있던지 찾아오는 구나.
* [다음] -> Conversation3

=== Conversation3
#actor Moa
뜬금 없게 왜 이래 당연하잖아?
* [다음] -> Conversation4

=== Conversation4
#actor Pago
헤헤 그런가아~
* [다음] -> Conversation5

=== Conversation5
#actor Moa
징그럽게 뭐야.. 저리 떨어져!
    -> END
