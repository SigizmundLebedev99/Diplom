export default [
    {code:'EPIC', color:'purple', name:'Epic', children:'EPIC!'},
    {code:'STORY', color:'green',name:'User Story',parent:'EPIC', children:'TASK!'},
    {code:'TASK', color:'blue', name:'Task',parent:'STORY',epicLink:true},
    {code:'BUG', color:'red',name:'Bug',parent:'STORY',epicLink:true},
    {code:'ISSUE', color:'yellow', name:'Issue',parent:'STORY',epicLink:true},
    {code:'SUBTASK', color:'blue', name:'Sub Task',parent:'TASK!', requireParent:true}
]