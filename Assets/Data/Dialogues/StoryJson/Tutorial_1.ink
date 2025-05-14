    -> Tutorial_1
    
=== Tutorial_1
#actor Moa
#UIMode Out
자! 그럼 지금부터 뭔가 먹을 것을 나한테 바치는
영광을 허락하겠어!
*[다음] -> Conversation1


=== Conversation1
#actor Pago
자.. 잠깐만!  내 말 좀 들어줘!
*[다음] -> Conversation2

=== Conversation2
#actor Moa
응? 할 말이 있어?
*[다음] -> Conversation3

=== Conversation3
#actor Pago
... 당연하지!
아까부터 계속 너 할 말만 하잖아!
*[다음] -> Conversation4

=== Conversation4
#actor Moa
내가?
*[다음] -> Conversation5

=== Conversation5
#actor Pago
그래!
*[다음] -> Conversation6

=== Conversation6
#actor Moa
뭐, 좋아
하고 싶은 말이 있다면 하도록 해
*[다음] -> Conversation7

=== Conversation7
#actor Pago
넌 누구야? 이 옷은 또 뭐고? 
나.. 나한테 무슨 짓을 한 거야!
*[다음] -> Conversation8
 
=== Conversation8
#actor Moa
질문은 한 번에 하나만!
그나저나 흐음... 너희는 전부 우리를 아는 거 아니었어?
*[다음] -> Conversation9

=== Conversation9
#actor Pago
너희?
*[다음] -> Conversation10

=== Conversation10
#actor Moa
그래 너희들! 방법은 잘 모르겠지만, 
나를 계속 귀찮게 만들었잖아! 
*[다음] -> Conversation11

=== Conversation11
#actor Pago
..?
*[다음] -> Conversation12

=== Conversation12
#actor Pago
너를 귀찮게 했다고?
*[다음] -> Conversation13

=== Conversation13
#actor Moa
흥, 너도 모르는구나
뭐, 상관없어. 관심도 없고
*[다음] -> Conversation14

=== Conversation14
#actor Moa
모른다니 한 번만 알려줄게. 
그 머릿속에 똑똑히 기억하도록 해
*[다음] -> Conversation15

=== Conversation15
#actor Moa
으흠 으흠
*[다음] -> Conversation16

=== Conversation16
#actor Moa
내 이름은 모아! 
지고하고 위대하신 만물의 아버지의 두 번째 자손!
*[다음] -> Conversation17

=== Conversation17
#actor Moa
만물의 아버지께서 내리신 사명을 다하기 위해 
전 우주를 돌아다니는 중이야!
*[다음] -> Conversation18

=== Conversation18
#actor Moa
어때어때 대단하지?
*[다음] -> Conversation19

=== Conversation19
#actor Pago
어... 그래. 모아 엄청 대단해
*[다음] -> Conversation20

=== Conversation20
#actor Moa
그치그치? 나도 그렇게 생각해!
그럼 어서 뭔가 먹을 것을 나에게 바치지 않을래?
*[다음] -> Conversation21

=== Conversation21
#actor Pago
먹을 거? 저기... 다른 질문은?
*[다음] -> Conversation22

=== Conversation22
#actor Moa
나 여기까지 오느라 엄청 힘들었단 말이야!
지금은 우선 뭐든 먹어야겠어!
*[다음] -> Conversation23

=== Conversation23
#actor Pago
아... 알았어
먹을 거 주면 질문 대답해줄 거지?
*[다음] -> Conversation24

=== Conversation24
#actor Moa
응!
*[수락] -> Accept

=== Accept
#quest tutorial_1
모아는 먼 길을 오느라 매우 배고픈 상태다.
뭔가 먹을 것을 주자. 근데... 모아는 뭘 먹지?
    -> END
