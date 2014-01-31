function AppDataModel() {
    var self = this,
        // Routes
        addExternalLoginUrl = "/api/Account/AddExternalLogin",
        changePasswordUrl = "/api/Account/changePassword",
        loginUrl = "/Token",
        logoutUrl = "/api/Account/Logout",
        registerUrl = "/api/Account/Register",
        registerExternalUrl = "/api/Account/RegisterExternal",
        removeLoginUrl = "/api/Account/RemoveLogin",
        setPasswordUrl = "/api/Account/setPassword",
        siteUrl = "/",
        userInfoUrl = "/api/Account/UserInfo";

    // Route operations
    function externalLoginsUrl(returnUrl, generateState) {
        return "/api/Account/ExternalLogins?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    function manageInfoUrl(returnUrl, generateState) {
        return "/api/Account/ManageInfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    function todoListUrl(id) { return "/api/TodoList/" + (id || ""); }

    function todoItemUrl(id) { return "/api/TodoItem/" + (id || ""); }

    // Other private operations
    function getSecurityHeaders() {
        var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }

        return {};
    }

    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null,
            headers: getSecurityHeaders()
        };

        return $.ajax(url, options);
    }

    // Operations
    self.clearAccessToken = function () {
        localStorage.removeItem("accessToken");
        sessionStorage.removeItem("accessToken");
    };

    self.setAccessToken = function (accessToken, persistent) {
        if (persistent) {
            localStorage["accessToken"] = accessToken;
        } else {
            sessionStorage["accessToken"] = accessToken;
        }
    };

    self.toErrorsArray = function (data) {
        var errors = new Array(),
            items;

        if (!data || !data.message) {
            return null;
        }

        if (data.modelState) {
            for (var key in data.modelState) {
                items = data.modelState[key];

                if (items.length) {
                    for (var i = 0; i < items.length; i++) {
                        errors.push(items[i]);
                    }
                }
            }
        }

        if (errors.length === 0) {
            errors.push(data.message);
        }

        return errors;
    };

    // Data
    self.returnUrl = siteUrl;

    // Data access operations
    self.addExternalLogin = function (data) {
        return $.ajax(addExternalLoginUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.changePassword = function (data) {
        return $.ajax(changePasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.getExternalLogins = function (returnUrl, generateState) {
        return $.ajax(externalLoginsUrl(returnUrl, generateState), {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.getManageInfo = function (returnUrl, generateState) {
        return $.ajax(manageInfoUrl(returnUrl, generateState), {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.getUserInfo = function (accessToken) {
        var headers;

        if (typeof (accessToken) !== "undefined") {
            headers = {
                "Authorization": "Bearer " + accessToken
            };
        } else {
            headers = getSecurityHeaders();
        }

        return $.ajax(userInfoUrl, {
            cache: false,
            headers: headers
        });
    };

    self.login = function (data) {
        return $.ajax(loginUrl, {
            type: "POST",
            data: data
        });
    };

    self.logout = function () {
        return $.ajax(logoutUrl, {
            type: "POST",
            headers: getSecurityHeaders()
        });
    };

    self.register = function (data) {
        return $.ajax(registerUrl, {
            type: "POST",
            data: data
        });
    };

    self.registerExternal = function (accessToken, data) {
        return $.ajax(registerExternalUrl, {
            type: "POST",
            data: data,
            headers: {
                "Authorization": "Bearer " + accessToken
            }
        });
    };

    self.removeLogin = function (data) {
        return $.ajax(removeLoginUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.setPassword = function (data) {
        return $.ajax(setPasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.setPassword = function (data) {
        return $.ajax(setPasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    // Data Access Operations for TodoItem
    self.getTodoLists = function getTodoLists(todoListsObservable, errorObservable) {
        return ajaxRequest("GET", todoListUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedTodoLists = $.map(data, function (list) { return new TodoList(self, list); });
            todoListsObservable(mappedTodoLists);
        }

        function getFailed() {
            errorObservable("Error retrieving todo lists.");
        }
    }

    self.saveNewTodoItem = function saveNewTodoItem(todoItem) {
        todoItem.errorMessage(null);
        return ajaxRequest("POST", todoItemUrl(), todoItem)
            .done(function (result) {
                todoItem.todoItemId = result.todoItemId;
            })
            .fail(function () {
                todoItem.errorMessage("Error adding a new todo item.");
            });
    }

    self.saveNewTodoList = function saveNewTodoList(todoList) {
        todoList.errorMessage(null);
        return ajaxRequest("POST", todoListUrl(), todoList)
            .done(function (result) {
                todoList.todoListId = result.todoListId;
                todoList.userId = result.userId;
            })
            .fail(function () {
                todoList.errorMessage("Error adding a new todo list.");
            });
    }

    self.deleteTodoItem = function deleteTodoItem(todoItem) {
        return ajaxRequest("DELETE", todoItemUrl(todoItem.todoItemId))
            .fail(function () {
                todoItem.errorMessage("Error removing todo item.");
            });
    }

    self.deleteTodoList = function deleteTodoList(todoList) {
        return ajaxRequest("DELETE", todoListUrl(todoList.todoListId))
            .fail(function () {
                todoList.errorMessage("Error removing todo list.");
            });
    }

    self.saveChangedTodoItem = function saveChangedTodoItem(todoItem) {
        todoItem.errorMessage(null);
        return ajaxRequest("PUT", todoItemUrl(todoItem.todoItemId), todoItem, "text")
            .fail(function () {
                todoItem.errorMessage("Error updating todo item.");
            });
    }

    self.saveChangedTodoList = function saveChangedTodoList(todoList) {
        todoList.errorMessage(null);
        return ajaxRequest("PUT", todoListUrl(todoList.todoListId), todoList, "text")
            .fail(function () {
                todoList.errorMessage("Error updating the todo list title. Please make sure it is non-empty.");
            });
    }
}
