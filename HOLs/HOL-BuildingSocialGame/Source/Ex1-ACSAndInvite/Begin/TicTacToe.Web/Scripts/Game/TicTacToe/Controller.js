
function TicTacToeController(viewModel, board, game) {
    this.viewModel = viewModel;
    this.board = board;
    this.game = game;

    var controller = this;

    this.board.onMove = function (x, y) { controller.onMove(x, y); };
};

TicTacToeController.prototype.start = function () {
    this.viewModel.currentColor(TTTColor.Cross);
    this.viewModel.playerColor(TTTColor.Cross);
};

TicTacToeController.prototype.onMove = function (x, y) {
    var color = this.viewModel.currentColor();

    if (!this.game.isValid(x, y, color))
        return;

    this.game.move(x, y, color);

    this.board.drawMove(x, y, color);

    this.updateGameStatus();
};

TicTacToeController.prototype.updateGameStatus = function () {

    if (this.game.isTie()) {
        this.viewModel.isTie(true);
        this.viewModel.currentColor(TTTColor.Empty);
        return;
    }

    if (this.game.hasWinner()) {
        this.viewModel.winnerColor(this.game.getWinner());
        this.viewModel.currentColor(TTTColor.Empty);
        return;
    }


    this.viewModel.currentColor(this.game.nextColor(this.viewModel.currentColor()));

    function nextColor(color) {
        if (color === TTTColor.Cross)
            return TTTColor.Circle;
        if (color === TTTColor.Circle)
            return TTTColor.Cross;
    }
};