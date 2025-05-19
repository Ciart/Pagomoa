-> Tutorial_3
    
=== Tutorial_3
#actor Moa
#UIMode Out
자! 그럼 지금부터 뭔가 먹을 것을 나한테 바치는
영광을 허락하겠어!
*[다음] -> Conversation1


=== Conversation1
#actor Pago
글고 보니 잊고 있었네
*[다음] -> Conversation2

=== Conversation2
#actor Moa
잊어버린 거?
*[다음] -> Conversation3

=== Conversation3
#actor Pago
응. 네가 우리 집 부쉈잖아
*[다음] -> Conversation4

=== Conversation4
#actor Moa
집? 내가?
*[다음] -> Conversation5

=== Conversation5
#actor Pago
그래!
*[다음] -> Conversation6

=== Conversation6
#actor Moa
언제?
*[다음] -> Conversation7

=== Conversation7
#actor Pago
너가 처음 여기 떨어졌을 때 말이야!
너가 떨어진 곳이 우리 집이었다구!
*[다음] -> Conversation8

=== Conversation8
#actor Moa
그게? 그게 집이었다고?
내가 아는 집이랑은 좀 다른데...
*[다음] -> Conversation9

=== Conversation9
#actor Moa
집이 왜 그렇게 생겼어?
*[다음] -> Conversation10

=== Conversation10
#actor Pago
어... 그게... 그러니까...
집보다는 천막이지만 말이야
*[다음] -> Conversation11

=== Conversation11
#actor Moa
아하! 난 또 집이라길래 혹시나 했지
떨어질 때 뭐 걸리는 게 하나도 없었거든!
*[다음] -> Conversation12

=== Conversation12
#actor Moa
근데 파고는 왜 집에서 안 살고 천막에서 살아?
*[다음] -> Conversation13

=== Conversation13
#actor Pago
어... 그게... 사정이 있어서...
*[다음] -> Conversation14

=== Conversation14
#actor Moa
그렇구나! 
*[다음] -> Conversation15

=== Conversation15
#actor Pago
크흠.. 흠, 흠. 
어쨌든 밤이 오기 전에 텐트 수리해야 되는데...
*[다음] -> Conversation16

=== Conversation16
#actor Pago
수리 도구가 어딘가 있으려나?
*[수락] -> Accept

=== Accept
#actor Pago
#quest tutorial_3
밤이 오기 전에 텐트를 수리하자
텐트 수리를 하려면 도구가 있어야 하는데...
    -> END
