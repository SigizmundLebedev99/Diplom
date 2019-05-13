export default {
    EPIC:{color:'purple', name:'Epic', children:'EPIC!'},
    STORY:{color:'orange ',name:'User Story',parent:'EPIC', children:'TASK!'},
    TASK:{color:'blue', name:'Task',parent:'STORY',epickLink:true,assignable:true,children:'SUBTASK'},
    BUG:{color:'red',name:'Bug',parent:'STORY',epickLink:true,assignable:true,children:'SUBTASK'},
    ISSUE:{color:'yellow', name:'Issue',parent:'STORY',epickLink:true,assignable:true,children:'SUBTASK'},
    SUBTASK:{color:'green accent-4', name:'Sub Task',parent:'TASK!', requireParent:true,assignable:true}
}