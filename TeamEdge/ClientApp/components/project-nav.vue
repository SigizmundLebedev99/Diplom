<template>
    <v-layout row>
        <v-badge color="red" v-if="invites.length > 0">
            <template v-slot:badge>
                <span>!</span>
            </template>
            <v-icon large color="grey" tooltip="Приглашения">
                mail
            </v-icon>
        </v-badge>
        <v-badge color="blue" v-if="invites.length > 0">
            <template v-slot:badge>
                <span>{{projects.length}}</span>
            </template>
            <v-icon large color="grey" tooltip="Проекты">
                work
            </v-icon>
        </v-badge>
        <v-subheader>{{profile.fullName}}</v-subheader>
        <v-avatar size="36px" class="mr-3">
            <img v-if="profile.avatar"
                :src="profile.avatar"
                alt="Avatar">
            <v-icon large dark v-else>account_circle</v-icon>
        </v-avatar>
    </v-layout>          
</template>

<script>
import {mapGetters, mapState} from 'vuex'

export default {
    data(){
        return{
            ...mapState({
                invites:'projects/invites',
                projects:'projects/projects'
            })
        }
    },
    computed:{
        ...mapGetters({
            profile:'auth/profile'
        })
    }
}
</script>
