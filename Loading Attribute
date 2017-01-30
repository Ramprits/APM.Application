"use strict";

angular.module("auditComponents", ["auditServices"])
    .directive("auditList", [
        "$templateCache",
        "$compile",
        "auditService",
        "$rootScope",
        function ($templateCache, $compile, auditService, $rootScope) {
            return {
                restrict: "A",
                scope: {
                    apiEndpoint: "@",
                    templateId: "@",
                    loc: "=",
                    sortBy: "@",
                    sortReverse: "=",
                    filter: "="
                },
                link: function (scope, element) {

                    // Set default values
                    scope.pageSizeOptions = [10, 20, 50, 100];
                    scope.pageSize = scope.pageSizeOptions[0];
                    scope.pageNumber = 1;
                    scope.totalPages = null;
                    scope.totalCount = null;

                    scope.getStartIndex = function () {
                        if (scope.pageSize === null)
                            return 1;
                        return (scope.pageNumber - 1) * scope.pageSize + 1;
                    }

                    scope.$watch("filter", function () { scope.refresh(); }, true);

                    scope.getEndIndex = function () {
                        if (scope.pageSize === null)
                            return scope.totalCount;
                        var result = (scope.pageNumber - 1) * scope.pageSize + scope.pageSize;
                        if (scope.totalCount < result)
                            return scope.totalCount;
                        return result;
                    };

                    scope.getPageNumbers = function () {
                        return new Array(scope.totalPages);
                    };

                    scope.changePage = function (page) {
                        if (page > 0 && page <= scope.totalPages) {
                            scope.pageNumber = page;
                            scope.refresh();
                        }
                    };

                    scope.changePageSize = function () {
                        scope.pageNumber = 1;
                        scope.refresh();
                    };

                    scope.refresh = function () {
                        auditService.get(scope.apiEndpoint, scope.pageSize, scope.pageNumber, scope.filter, scope.sortBy, scope.sortReverse)
                            .then(function (response) {
                                var result = response.data;
                                scope.entries = result.entries;
                                scope.totalPages = result.totalPages;
                                scope.totalCount = result.totalCount;
                            });
                    };

                    scope.isInRole = $rootScope.isInRole;

                    scope.sorting = function (name) {
                        if (scope.sortBy === name) {
                            scope.sortReverse = !scope.sortReverse;
                        } else {
                            scope.sortBy = name;
                            scope.sortReverse = false;
                        }
                        scope.refresh();
                    };

                    scope.setSortClass = function (columnName) {
                        if (scope.sortBy === columnName) {
                            return scope.sortReverse ? "sorting_desc" : "sorting_asc";
                        }
                        return "sorting";
                    };

                    scope.delete = function (id) {
                        if (confirm("Are you sure to delete this entry?")) {
                            auditService.delete(scope.apiEndpoint, id)
                                .then(function (response) {
                                    toastr.success(scope.loc.alertDelete);
                                    scope.refresh();
                                },
                                    function (error) {
                                        if (error.status === 500) {
                                            toastr.warning("Tansaction Records Exist.<br/> You can't delete this record.");
                                            $state.go(".", { id: "" }, { reload: true });
                                        }
                                    }
                                );
                        }
                    }

                    var initialize = function () {
                        var template = $templateCache.get(scope.templateId);
                        element.replaceWith($compile(template)(scope));
                        scope.refresh();
                    };

                    initialize();
                }
            }
        }
    ])
     .directive("auditChildList",
    [
        "$templateCache",
        "$compile",
        "auditService", "applicationSettings",
        function ($templateCache, $compile, auditService, applicationSettings) {
            return {
                restrict: "A",
                scope: {
                    param: "=",
                    apiEndpoint: "@",
                    templateId: "@",
                    loc: "=",
                    user: "=",
                    mode: "=",
                    setState: '&'
                },
                link: function (scope, element) {
                    scope.pageSizeOptions = [10, 20, 50, 100];
                    scope.pageSize = scope.pageSizeOptions[0];
                    scope.pageNumber = 1;
                    scope.sortReverse = true;
                    scope.totalPages = null;
                    scope.totalCount = null;
                    scope.filterBy = {};
                    scope.FileType = "";

                    scope.getStartIndex = function () {
                        if (scope.pageSize === null)
                            return 1;
                        var result = (scope.pageNumber - 1) * scope.pageSize + 1;
                        if (scope.totalCount < result)
                            return scope.totalCount;
                        return result;
                    }

                    scope.getEndIndex = function () {
                        if (scope.pageSize === null)
                            return scope.totalCount;
                        var result = (scope.pageNumber - 1) * scope.pageSize + scope.pageSize;
                        if (scope.totalCount < result)
                            return scope.totalCount;
                        return result;
                    }

                    scope.getPageNumbers = function () {
                        return new Array(scope.totalPages);
                    }

                    scope.changePage = function (page) {
                        if (page > 0 && page <= scope.totalPages) {
                            scope.pageNumber = page;
                            scope.refresh();
                        }
                    }

                    scope.changePageSize = function () {
                        scope.pageNumber = 1;
                        scope.refresh();
                    }
                    scope.sorting = function (name) {
                        scope.sortBy = name;
                        scope.sortReverse = !scope.sortReverse;
                        scope.refresh();
                    };

                    function makePage(number, text, isActive) {
                        return {
                            number: number,
                            text: text,
                            active: isActive
                        };
                    }

                    scope.getPageNumbers = function () {
                        var
                            boundaryLinkNumbers = false,
                            rotate = true,
                            forceEllipses = false;
                        scope.pages = [];
                        var maxSize = 10;
                        // Default page limits
                        var startPage = 1, endPage = scope.totalPages;
                        var isMaxSized = angular.isDefined(maxSize) && maxSize < scope.totalPages;

                        // recompute if maxSize
                        if (isMaxSized) {
                            if (rotate) {
                                // Current page is displayed in the middle of the visible ones
                                startPage = Math.max(scope.pageNumber - Math.floor(maxSize / 2), 1);
                                endPage = startPage + maxSize - 1;

                                // Adjust if limit is exceeded
                                if (endPage > scope.totalPages) {
                                    endPage = scope.totalPages;
                                    startPage = endPage - maxSize + 1;
                                }
                            } else {
                                // Visible pages are paginated with maxSize
                                startPage = (Math.ceil(scope.pageNumber / maxSize) - 1) * maxSize + 1;

                                // Adjust last page if limit is exceeded
                                endPage = Math.min(startPage + maxSize - 1, scope.totalPages);
                            }
                        }

                        // Add page number links
                        for (var number = startPage; number <= endPage; number++) {
                            var page = makePage(number, number, number === scope.pageNumber);
                            scope.pages.push(page);
                        }

                        // Add links to move between page sets
                        if (isMaxSized && maxSize > 0 && (!rotate || forceEllipses || boundaryLinkNumbers)) {
                            if (startPage > 1) {
                                if (!boundaryLinkNumbers || startPage > 3) {
                                    //need ellipsis for all options unless range is too close to beginning
                                    var previousPageSet = makePage(startPage - 1, '...', false);
                                    scope.pages.unshift(previousPageSet);
                                }
                                if (boundaryLinkNumbers) {
                                    if (startPage === 3) {
                                        //need to replace ellipsis when the buttons would be sequential
                                        var secondPageLink = makePage(2, '2', false);
                                        scope.pages.unshift(secondPageLink);
                                    }
                                    //add the first page
                                    var firstPageLink = makePage(1, '1', false);
                                    scope.pages.unshift(firstPageLink);
                                }
                            }

                            if (endPage < scope.totalPages) {
                                if (!boundaryLinkNumbers || endPage < scope.totalPages - 2) {
                                    //need ellipsis for all options unless range is too close to end
                                    var nextPageSet = makePage(endPage + 1, '...', false);
                                    scope.pages.push(nextPageSet);
                                }
                                if (boundaryLinkNumbers) {
                                    if (endPage === scope.totalPages - 2) {
                                        //need to replace ellipsis when the buttons would be sequential
                                        var secondToLastPageLink =
                                            makePage(scope.totalPages - 1, scope.totalPages - 1, false);
                                        scope.pages.push(secondToLastPageLink);
                                    }
                                    //add the last page
                                    var lastPageLink = makePage(scope.totalPages, scope.totalPages, false);
                                    scope.pages.push(lastPageLink);
                                }
                            }
                        }
                    }

                    scope.getIcon = function (column) {
                        if (scope.sortBy === column) {
                            return scope.sortReverse
                                ? 'fa fa-sort-asc'
                                : 'fa fa-sort-desc';
                        }

                        return '';
                    }

                    scope.refresh = function () {
                        auditService.getChild(scope.apiEndpoint,
                                scope.pageSize,
                                scope.pageNumber,
                                scope.param,
                                scope.filterBy,
                                scope.sortBy,
                                scope.sortReverse)
                            .then(function (response) {
                                scope.entries = response.data.entries;
                                scope.totalPages = response.data.totalPages;
                                scope.totalCount = response.data.totalCount;
                                scope.getPageNumbers();
                            });
                    };

                    ////////////////////////////////////////////////////
                    scope.delete = function (routeUrl, id) {
                        if (confirm("Are you sure to delete this entry?")) {
                            auditService.delete(routeUrl, id)
                                .then(function (response) {
                                    toastr.success(scope.loc.alertDelete);
                                    scope.refresh();
                                    scope.setState();
                                });
                        }
                    }


                    var initialize = function () {
                        var template = $templateCache.get(scope.templateId);
                        element.replaceWith($compile(template)(scope));
                        scope.refresh();
                    }

                    initialize();
                }
            }
        }
    ])
//.directive('customDatepicker',
//        function () {
//            return {
//                restrict: "A",
//                require: "ngModel",
//                link: function (scope, element, attrs, ngModelCtrl) {

//                    $(function () {
//                        element.datepicker({
//                            dateFormat: 'dd.mm.yy',
//                            changeMonth: true,
//                            changeYear: true,
//                            onSelect: function (date) {
//                                scope.$apply(function () {
//                                    ngModelCtrl.$setViewValue(date);
//                                });

//                                scope.$apply(attrs.getWeek);
//                            }
//                        });
//                    });

//                }
//            }
//        })
    .directive('loading',
        [
      '$http', function ($http) {
          return {
              restrict: 'A',
              link: function (scope, elm) {
                  scope.isLoading = function () {
                      return $http.pendingRequests.length > 0;
                  };
                  elm.hide();
                  scope.$watch(scope.isLoading,
                      function (v) {
                          if (v) {
                              elm.show();
                          } else {
                              elm.hide();
                          }
                      });
              }
          };

      }
        ])
     .directive('myFormSubmit', function () {
         return {
             require: '^form',
             scope: {
                 callback: '&myFormSubmit'
             },
             link: function (scope, element, attrs, form) {
                 element.bind('click', function (e) {
                     if (form.$valid) {
                         scope.callback();
                     }
                 });
             }
         };
     })
.directive('contenteditable', function () {
    return {
        restrict: 'A', // only activate on element attribute
        require: '?ngModel', // get a hold of NgModelController
        link: function (scope, element, attrs, ngModel) {
            if (!ngModel) return; // do nothing if no ng-model

            // Specify how UI should be updated
            ngModel.$render = function () {
                element.html(ngModel.$viewValue || '');
            };

            // Listen for change events to enable binding
            element.on('blur keyup change', function () {
                scope.$apply(read);
            });
            read(); // initialize

            // Write data to the model
            function read() {
                var html = element.html();
                // When we clear the content editable the browser leaves a <br> behind

                ngModel.$setViewValue(html);
            }
        }
    };
});
