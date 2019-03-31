export default [
    {code:'EPICK', color:'purple', name:'Epick', children:'EPICK!'},
    {code:'STORY', color:'green',name:'User Story',parent:'EPICK', children:'TASK!'},
    {code:'TASK', color:'blue', name:'Task',parent:'STORY',epickLink:true},
    {code:'BUG', color:'red',name:'Bug',parent:'STORY',epickLink:true},
    {code:'ISSUE', color:'yellow', name:'Issue',parent:'STORY',epickLink:true},
    {code:'SUB', color:'blue', name:'Sub Task',parent:'TASK!', requireParent:true}
]