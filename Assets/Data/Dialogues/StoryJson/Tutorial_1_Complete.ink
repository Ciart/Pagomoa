-> Tutorial_1_RestoreTent_Complete

=== Tutorial_1_RestoreTent_Complete
#actor Pago
#UIMode Out
혹시... 너가 말한 게 이거야?
*[다음] -> Conversation1

=== Conversation1
#actor Moa
와! 맞아! 그거야 그거!
*[수락] -> Accept

=== Accept
# reward tutorial_1
(어... 음... 이걸 먹는다고? 
뭐... 저렇게 좋아하는데 상관없겠지...)
    -> END
