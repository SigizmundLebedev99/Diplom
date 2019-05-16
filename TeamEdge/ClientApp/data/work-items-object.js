export default {
    EPIC:{color:'purple', name:'Epic', children:'EPIC!'},
    STORY:{color:'orange accent-4',name:'User Story',parent:'EPIC', children:'TASK!'},
    TASK:{color:'blue', name:'Task',parent:'STORY',epicLink:true,assignable:true,children:'SUBTASK',timesheet:true},
    BUG:{color:'red',name:'Bug',parent:'STORY',epicLink:true,assignable:true,children:'SUBTASK',timesheet:true},
    ISSUE:{color:'teal darken-1', name:'Issue',parent:'STORY',epicLink:true,assignable:true,children:'SUBTASK',timesheet:true},
    SUBTASK:{color:'green accent-4', name:'Sub Task',parent:'TASK!', requireParent:true,assignable:true,timesheet:true}
}