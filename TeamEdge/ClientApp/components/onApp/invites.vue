<template>
    <v-conteiner>
        <span class="title">Приглашения</span>
        <v-layout row wrap justify-center align-start>
            <v-card v-for="(invite,i) in invites" :key="i" dark width="220px" class="mr-4 elevation-12">
                <v-card-text>
                    <v-layout align-center column>
                        <v-chip>
                            <v-avatar>
                                <v-icon v-if="!invite.from.avatar" size="80" light color="primary">account_circle</v-icon>
                                <img v-else :src="invite.from.avatar"/>
                            </v-avatar>
                            {{invite.from.fillName}}
                        </v-chip>
                        кинул вам инвайт в проект
                        <v-avatar size="100" class="mb-3" color="white">
                            <v-icon v-if="!invate.logo" size="80" light color="primary">work</v-icon>
                            <img v-else :src="invate.logo"/>
                        </v-avatar>
                        <span>{{invite.projectName}}</span>
                    </v-layout>
                </v-card-text>
                <v-footer dense color="primary" class="px-0 py-0" flat>
                    <v-spacer/>
                    <v-btn small class="primary">Принять</v-btn>
                </v-footer>
            </v-card>
        </v-layout>
    </v-conteiner>
</template>

<script>
export default {

}
</script>

<style>

</style>
