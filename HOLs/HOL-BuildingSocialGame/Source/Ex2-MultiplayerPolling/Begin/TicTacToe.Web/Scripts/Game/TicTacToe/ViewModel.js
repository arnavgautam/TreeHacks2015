
TTTColor = { Empty: 0, Cross: 1, Circle: 2 };

function TicTacToeViewModel() {
    this.playerColor = ko.observable(TTTColor.Empty);
    this.winnerColor = ko.observable(TTTColor.Empty);
    this.currentColor = ko.observable(TTTColor.Cross);
    this.isTie = ko.observable(false);
}