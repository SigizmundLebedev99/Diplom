<template>
    <div class="constr scroll-x overflow-x-hidden">
        <span v-if="!timesheet.length">Статус задачи не изменялся</span>
        <div v-for="(t,i) in timesheet" :key="i" class="my-2 pl-2 bordered">
            <v-layout wrap align-center class="mb-1">
                <span class="date-text mr-3">{{getDateTime(t.startDate)}}</span>
                <user-label :user="t.changedBy"/>
                <span class="mx-1">начал выполнение</span>
                <span v-if="!t.subtask"> задачи</span>
                <item-chip v-else :item="t.subtask" :withoutRoute="true"/>
            </v-layout>
            <v-layout wrap align-center v-if="t.endDate">
                <span class="date-text mr-3">{{getDateTime(t.endDate)}}</span>
                <user-label :user="t.endedBy"/>
                <span class="mx-1">{{t.endsWith == 3?"завершил":"приостановил"}} выполнение </span>
                <span v-if="!t.subtask"> задачи</span>
                <item-chip v-else :item="t.subtask" :withoutRoute="true"/>
            </v-layout>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapActions} from 'vuex'
import userLabel from './user-label'
import itemChip from './item-chip'
export default {
    components:{
        'user-label':userLabel,
        'item-chip':itemChip
    },
    mounted(){
        this.fetchTimesheet(this.currentWI.descriptionId);
    },
    computed:{
        ...mapGetters({
            currentWI:"project/currentWI",
            loading:'timesheet/loading',
            timesheet:'timesheet/timesheet'
        })
    },
    methods:{
        ...mapActions({
            fetchTimesheet:'timesheet/fetchTimesheet'
        })
    },
    watch:{
        currentWI(to, from){
            if(to)
                this.fetchTimesheet(this.currentWI.descriptionId);
        }
    }
}
</script>

<style scoped>
.bordered{
    border-left: 3px solid grey
}
.constr{
    max-height: 170px;
}
</style>
