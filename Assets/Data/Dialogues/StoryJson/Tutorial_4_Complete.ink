-> Tutorial_4_Complete
    
=== Tutorial_4_Complete
#actor MoMo
#UIMode Out
고맙네. 여긴 대금일세. 넉넉하게 넣었으니 확인해 보게. 
*[다음] -> Conversation1

=== Conversation1
#actor Moa
우와 웬 돈이야? 나 없을 때 뭐 했어?
*[다음] -> Conversation2

=== Conversation2
#actor Pago
아 모모씨가 부탁 하셔서. 구리를 가져다 드렸어.
*[다음] -> Conversation3

=== Conversation3
#actor Moa
뭐? 구리를 팔았다구? 그럼 내가 먹을 게 줄어들잖아!
몇 개나 팔았는데?
*[다음] -> Conversation4

=== Conversation4
#actor Pago
아냐아냐 별로 안 팔았어.
*[다음] -> Conversation5

=== Conversation5
#actor Pago
당연히 모아가 먹을 건 남겨두고 팔았지.
그러니까 진정해.
*[다음] -> Conversation6

=== Conversation6
#actor Moa
진짜야? 진짜진짜지?
*[다음] -> Conversation7

=== Conversation7
#actor Pago
응, 진짜진짜. 그렇죠 모모씨?
*[다음] -> Conversation8

=== Conversation8
#actor MoMo
예, 한 치의 거짓 없는 사실입니다.
*[다음] -> Conversation9

=== Conversation9
#actor Moa
진짜진짜로?
*[다음] -> Conversation10

=== Conversation10
#actor MoMo
예, 진짜진짜 입니다. 
*[다음] -> Conversation11

=== Conversation11
#actor Moa
으으음..
*[다음] -> Conversation12

=== Conversation12
#actor Pago
미안해. 모아가 싫으면 다시는 안 그럴게.
*[다음] -> Accept

=== Accept
#actor Moa
# reward tutorial_4
아니야.  그렇게 맛있는 것도 아니었으니까.
그냥. 내가 먹을 것만 남겨둬.
    -> END
    
    
    
    