export default {
    EPICK:{color:'purple', name:'Epick', children:'EPICK!'},
    STORY:{color:'orange ',name:'User Story',parent:'EPICK', children:'TASK!'},
    TASK:{color:'blue', name:'Task',parent:'STORY',epickLink:true,assignable:true},
    BUG:{color:'red',name:'Bug',parent:'STORY',epickLink:true,assignable:true},
    ISSUE:{color:'yellow', name:'Issue',parent:'STORY',epickLink:true,assignable:true},
    SUBTASK:{color:'green accent-4', name:'Sub Task',parent:'TASK!', requireParent:true,assignable:true}
}