export default {
    EPICK:{color:'purple', name:'Epick', children:'EPICK!'},
    STORY:{color:'orange ',name:'User Story',parent:'EPICK', children:'TASK!'},
    TASK:{color:'blue', name:'Task',parent:'STORY',epickLink:true,assignable:true,children:'SUBTASK'},
    BUG:{color:'red',name:'Bug',parent:'STORY',epickLink:true,assignable:true,children:'SUBTASK'},
    ISSUE:{color:'yellow', name:'Issue',parent:'STORY',epickLink:true,assignable:true,children:'SUBTASK'},
    SUBTASK:{color:'green accent-4', name:'Sub Task',parent:'TASK!', requireParent:true,assignable:true}
}