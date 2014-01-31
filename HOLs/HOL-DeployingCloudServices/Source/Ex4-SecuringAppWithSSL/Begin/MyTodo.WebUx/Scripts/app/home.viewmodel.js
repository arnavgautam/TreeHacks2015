function HomeViewModel(app, dataModel) {
    // Private state
    var self = this;
    var showTodoList = function (todoList) {
        self.todoLists.unshift(todoList); // Insert new todoList at the front
    };

    // Private operations
    function initialize() {
        dataModel.getTodoLists(self.todoLists, self.error); // load todoLists
    }

    // Data
    self.todoLists = ko.observableArray();
    self.error = ko.observable();

    // Operations
    self.addTodoList = function () {
        var todoList = new TodoList(dataModel);
        todoList.isEditingListTitle(true);
        dataModel.saveNewTodoList(todoList)
            .then(addSucceeded)
            .fail(addFailed);

        function addSucceeded() {
            showTodoList(todoList);
        }
        function addFailed() {
            self.error("Save of new todoList failed");
        }
    };

    self.deleteTodoList = function (todoList) {
        self.todoLists.remove(todoList);
        dataModel.deleteTodoList(todoList)
            .fail(deleteFailed);

        function deleteFailed() {
            showTodoList(todoList); // re-show the restored list
        }
    };

    initialize();
}

function TodoItem(dataModel, data) {
    var self = this;
    data = data || {};

    // Persisted properties
    self.todoItemId = data.todoItemId;
    self.title = ko.observable(data.title).extend({ required: true });
    self.isDone = ko.observable(data.isDone);
    self.todoListId = data.todoListId;

    // Non-persisted properties
    self.errorMessage = ko.observable();
    self.validationErrors = ko.validation.group([self.title]);

    saveChanges = function () {
        if (self.validationErrors().length > 0) {
            self.validationErrors.showAllMessages();
            return;
        }

        return dataModel.saveChangedTodoItem(self);
    };

    // Auto-save when these properties change
    self.isDone.subscribe(saveChanges);
    self.title.subscribe(saveChanges);

    self.toJson = function () { return ko.toJSON(self) };
};

function TodoList(dataModel, data) {
    var self = this;
    data = data || {};

    // convert raw todoItem data objects into array of TodoItems
    var importTodoItems = function importTodoItems(todoItems) {
        /// <returns value="[new todoItem()]"></returns>
        return $.map(todoItems || [],
                function (todoItemData) {
                    return new TodoItem(dataModel, todoItemData);
                });
    }

    // Persisted properties
    self.todoListId = data.todoListId;
    self.userId = data.userId || "to be replaced";
    self.title = ko.observable(data.title || "My todos").extend({ required: true });
    self.todos = ko.observableArray(importTodoItems(data.todos));

    // Non-persisted properties
    self.isEditingListTitle = ko.observable(false);
    self.newTodoTitle = ko.observable();
    self.errorMessage = ko.observable();
    self.validationErrors = ko.validation.group([self.title]);

    self.deleteTodo = function () {
        var todoItem = this;
        return dataModel.deleteTodoItem(todoItem)
             .done(function () { self.todos.remove(todoItem); });
    };

    // Auto-save when these properties change
    self.title.subscribe(function () {

        if (self.validationErrors().length > 0) {
            self.validationErrors.showAllMessages();
            return;
        }

        return dataModel.saveChangedTodoList(self);
    });

    self.toJson = function () { return ko.toJSON(self) };

    self.addTodo = function () {
        if (self.newTodoTitle()) { // need a title to save
            var todoItem = new TodoItem(dataModel,
                {
                    title: self.newTodoTitle(),
                    todoListId: self.todoListId
                });
            self.todos.push(todoItem);
            dataModel.saveNewTodoItem(todoItem);
            self.newTodoTitle("");
        }
    };
};

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});
